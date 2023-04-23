using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.WebSockets.Broker._DI;
using Services.Communication.WebSockets.Broker.Reliability.Abstract;

namespace Services.Communication.WebSockets.Broker.Reliability._DI
{
    public static class ReliabilityCommunicatorConfiguration
    {
        public static IServiceCollection RegisterReliabilityCommunicator(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterSocketCommunicator();
            services.RegisterInMemoryCaching();

            services.RegisterHttpAuthorizationCommunicators();
            services.AddSingleton<IReliabilityCommunicator, ReliabilityCommunicator>();

            return services;
        }
    }
}
