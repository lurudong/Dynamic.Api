using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Data;
using System.Text.RegularExpressions;

namespace Dynamic
{
    //自定义应用模型转换
    //这个实现动态API比较主要的
    public class CustomApplicationModelConvention : IApplicationModelConvention
    {

        private Dictionary<string, List<string>> _httpVeDic = new Dictionary<string, List<string>>();

        public CustomApplicationModelConvention()
        {

            _httpVeDic.Add("GET", new List<string>() { "Find", "Query", "GetAll" });

            _httpVeDic.Add("POST", new List<string>() { "Create", "Insert", "Add" });

            _httpVeDic.Add("PUT", new List<string>() { "Update", "Modify" });
            _httpVeDic.Add("DELETE", new List<string>() { "Remove" });
        }
        public const string CentralPrefix = "api/";
        public void Apply(ApplicationModel application)
        {

            foreach (var controller in application.Controllers)
            {

                var controllerType = controller.ControllerType.AsType();
                //是否继承IDynamicService
                if (typeof(IDynamicService).IsAssignableFrom(controllerType))
                {


                    controller.ControllerName = PostSuffix(controller.ControllerName, "Service");
                    ConfigureSelector(controller);
                }
            }

        }


        private void ConfigureSelector(ControllerModel controller)
        {
            if (controller.Selectors.Any((selector => selector.AttributeRouteModel != null)))
            {

                return;
            }

            foreach (var item in controller.Actions)
            {
                CreateActionSelector(controller.ControllerName, item);
            }
        }

        private void CreateActionSelector(string controllerName, ActionModel action)
        {


            var actionName = action.ActionName;

            if (!action.Selectors.Any() || action.Selectors.Any(o => !o.ActionConstraints.Any()))
            {

                CreateSelectorModel(action, controllerName);
            }
            else
            {
                foreach (var selector in action.Selectors)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(CreateActionRoute(action, controllerName), selector.AttributeRouteModel); ;
                }
            }


        }
        private string GetHttpVerb(ActionModel action)
        {
            var actionName = GetPascalCaseFirstWord(action.ActionName);
            var httpVerbkey = _httpVeDic.Where(o => o.Value.Any(v => v.Contains(actionName))).FirstOrDefault().Key;
            return httpVerbkey;

        }
        private AttributeRouteModel CreateActionRoute(ActionModel action, string controllerName)
        {
            var routePath = $"{CentralPrefix}{controllerName}/{action.ActionName}";
            return new AttributeRouteModel(new RouteAttribute(routePath));
        }


        /// <summary>
        /// chargpt生成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetPascalCaseFirstWord(string input)
        {
            string[] words = Regex.Split(input, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");
            return words[1];
        }
        private void CreateSelectorModel(ActionModel action, string controllerName)
        {
            var httpMethod = GetHttpVerb(action);
            var selectorModel = action.Selectors[0];
            if (selectorModel.AttributeRouteModel is null)
            {
                selectorModel.AttributeRouteModel = CreateActionRoute(action, controllerName);
            }
            if (!selectorModel.ActionConstraints.Any())
            {
                selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));
            }

            switch (httpMethod?.ToLower())
            {
                case "get":
                    selectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                    break;
                case "post":
                    selectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                    break;
                case "put":
                    selectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                    break;
                case "delete":
                    selectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                    break;
                default:
                    throw new Exception("请求类型不存在");
            }

            //foreach (var item in action.Selectors)
            //{
            //    //给此Action添加路由
            //    item.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(routePath));
            //    //添加HttpMethod

            //    item.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { "get" }));

            //}

        }

        private string GetActionName(string actionName)
        {
            if (actionName.EndsWith("async", StringComparison.OrdinalIgnoreCase))
            {


                return actionName.Substring(0, actionName.Length - "Async".Length);
            }

            return actionName;
        }

        private string PostSuffix(string value, string suffix)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(suffix))
            {
                return value;
            }

            if (!value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }
            return value.Substring(0, value.Length - suffix.Length);
        }
    }
}