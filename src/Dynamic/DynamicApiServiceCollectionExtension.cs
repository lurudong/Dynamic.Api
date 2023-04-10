using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Dynamic
{
    public static class DynamicApiServiceCollectionExtension
    {

        public static IServiceCollection AddDynamicApi(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(op => op.Conventions.Insert(0, new CustomApplicationModelConvention()));
            var applicationPartManager = services.BuildServiceProvider().GetRequiredService<ApplicationPartManager>();
            if (applicationPartManager is null)
            {
                throw new Exception("没有注入ApplicationPartManager服务");
            }
            applicationPartManager.FeatureProviders.Add(new DynamicControlleFeatureProvider());
            return services;
        }
    }
}
