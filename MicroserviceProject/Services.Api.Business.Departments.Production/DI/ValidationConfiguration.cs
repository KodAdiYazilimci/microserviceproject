using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Production.Configuration.Validation.Product;
using Services.Api.Business.Departments.Production.Configuration.Validation.Production;
using Services.Api.Business.Departments.Production.Util.Validation.Product;
using Services.Api.Business.Departments.Production.Util.Validation.Production;

namespace Services.Api.Business.Departments.Production.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateProductRule>();
            services.AddSingleton<CreateProductValidator>();

            services.AddSingleton<ProduceProductRule>();
            services.AddSingleton<ProduceProductValidator>();

            return services;
        }
    }
}
