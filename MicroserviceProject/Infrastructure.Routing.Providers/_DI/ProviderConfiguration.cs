using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.Abstract;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Routing.Providers.DI
{
    public static class ProviderConfiguration
    {
        public static IServiceCollection RegisterRoutingProviders(this IServiceCollection services)
        {
            services.RegisterHttpRouteRepositories();
            services.RegisterInMemoryCaching();

            services.AddSingleton<IRouteProvider, SqlRouteProvider>();

            return services;
        }
    }
}
