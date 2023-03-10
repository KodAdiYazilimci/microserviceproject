using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Gateway.Abstract;

namespace Services.Communication.Http.Broker.Gateway._DI
{
    public static class GatewayConfiguration
    {
        public static IServiceCollection RegisterGatewayCommunicators(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterHttpAuthorizationCommunicators();

            services.AddSingleton<IAuthenticationCommunicator, AuthenticationCommunicator>();

            return services;
        }
    }
}
