using Infrastructure.Communication.Http.Broker.DI;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Configuration;
using Infrastructure.ServiceDiscovery.Providers;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.Registerers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceDiscovery.Register.DI
{
    public static class ServiceRegistererConfiguration
    {
        public static IServiceCollection RegisterServiceRegisterers(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<ISolidServiceConfiguration, AppConfigSolidServiceConfiguration>();
            services.AddSingleton<ISolidServiceProvider, AppConfigSolidServiceProvider>();
            services.AddSingleton<IServiceRegisterer, HttpServiceRegisterer>();

            return services;
        }
    }
}
