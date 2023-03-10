
using Infrastructure.Communication.Http.Broker.DI;
using Infrastructure.Routing.Providers.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.Abstract;
using Services.Communication.Http.Broker.DI;

namespace Services.Communication.Http.Broker.Authorization.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class CommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterHttpAuthorizationCommunicators(this IServiceCollection services)
        {
            services.RegisterDefaultCommunicator();
            services.RegisterHttpServiceCommunicator();
            services.RegisterRoutingProviders();

            services.AddSingleton<IAuthorizationCommunicator, AuthorizationCommunicator>();

            return services;
        }
    }
}
