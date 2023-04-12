using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Communication.Http.Broker.DI;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Configuration;
using Infrastructure.ServiceDiscovery.Discoverer.Discovers;
using Infrastructure.ServiceDiscovery.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceDiscovery.Discoverer.DI
{
    public static class ServiceDiscovererConfiguration
    {
        public static IServiceCollection RegisterServiceDiscoverers(this IServiceCollection services)
        {
            services.RegisterInMemoryCaching();
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<ISolidServiceConfiguration, AppConfigSolidServiceConfiguration>();
            services.AddSingleton<ISolidServiceProvider, AppConfigSolidServiceProvider>();
            services.AddSingleton<IServiceDiscoverer, HttpServiceDiscoverer>();

            return services;
        }
    }
}
