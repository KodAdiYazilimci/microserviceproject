using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.WebSockets.Broker._DI;
using Services.Communication.WebSockets.Broker.Security.Abstract;

namespace Services.Communication.WebSockets.Broker.Security._DI
{
    public static class SecurityCommunicatorConfiguration
    {
        public static IServiceCollection RegisterSecurityCommunicator(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterSocketCommunicator();
            services.RegisterInMemoryCaching();

            services.RegisterHttpAuthorizationCommunicators();
            services.AddSingleton<ISecurityCommunicator, SecurityCommunicator>();

            return services;
        }
    }
}
