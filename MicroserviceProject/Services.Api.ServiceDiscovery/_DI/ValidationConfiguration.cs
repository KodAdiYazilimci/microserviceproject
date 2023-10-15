using Services.Api.ServiceDiscovery.Configuration.Validation.Registry.Register;
using Services.Api.ServiceDiscovery.Util.Validation.Registry.Register;

namespace Services.Api.ServiceDiscovery._DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<RegisterRule>();
            services.AddSingleton<RegisterValidator>();

            return services;
        }
    }
}
