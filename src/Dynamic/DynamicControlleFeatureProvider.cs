using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Dynamic
{
    public class DynamicControlleFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();
            if ((typeof(IDynamicService).IsAssignableFrom(type)) && //判断基类型是否是Controller
                (typeInfo.IsPublic && !typeInfo.IsAbstract && !typeInfo.IsGenericType && !typeInfo.IsInterface))
            {
                return true;
            }
            return false;
        }
    }
}
