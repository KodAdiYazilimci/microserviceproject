using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Routing.Providers.DI
{
    public static class ProviderConfiguration
    {
        public static IServiceCollection RegisterRoutingProviders(this IServiceCollection services)
        {
            services.RegisterHttpRouteRepositories();
            services.RegisterInMemoryCaching();

            services.AddSingleton<RouteProvider>();

            return services;
        }
    }
}
