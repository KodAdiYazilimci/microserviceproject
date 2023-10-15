using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Storage.Configuration.Validation.Stock;
using Services.Api.Business.Departments.Storage.Util.Validation.Stock;

namespace Services.Api.Business.Departments.Storage.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateStockRule>();
            services.AddSingleton<CreateStockValidator>();

            services.AddSingleton<DescendStockRule>();
            services.AddSingleton<DescendStockValidator>();

            return services;
        }
    }
}
