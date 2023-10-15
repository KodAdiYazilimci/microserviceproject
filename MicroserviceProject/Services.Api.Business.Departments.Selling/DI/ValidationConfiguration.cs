using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Selling.Configuration.Validation.Selling;
using Services.Api.Business.Departments.Selling.Util.Validation.Selling;

namespace Services.Api.Business.Departments.Selling.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CreateSellingRule>();
            services.AddSingleton<CreateSellingValidator>();

            return services;
        }
    }
}
