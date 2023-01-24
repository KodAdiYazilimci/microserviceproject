using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Routing.Providers._DI
{
    public static class ProviderConfiguration
    {
        public static IServiceCollection RegisterRoutingProviders(this IServiceCollection services)
        {
            services.AddSingleton<RouteProvider>();

            return services;
        }
    }
}
