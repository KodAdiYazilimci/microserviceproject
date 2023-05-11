using Infrastructure.Communication.Http.Broker.DI;
using Infrastructure.ServiceDiscovery.Discoverer.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.DI;
using Services.Communication.Http.Broker.ServiceDiscovery.Abstract;

namespace Services.Communication.Http.Broker.ServiceDiscovery.DI
{
    public static class CommunicatorConfiguration
    {
        public static IServiceCollection RegisterHttpServiceDiscoveryCommunicators(this IServiceCollection services)
        {
            services.RegisterDefaultCommunicator();
            services.RegisterHttpServiceCommunicator();

            services.RegisterServiceDiscoverers();

            services.AddSingleton<IServiceDiscoveryCommunicator, ServiceDiscoveryCommunicator>();

            return services;
        }
    }
}
