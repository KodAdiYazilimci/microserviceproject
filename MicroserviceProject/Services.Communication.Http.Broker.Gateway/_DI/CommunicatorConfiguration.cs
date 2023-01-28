
using Infrastructure.Communication.Http.Broker.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization;
using Services.Communication.Http.Broker.Gateway.Public;

namespace Services.Communication.Http.Broker.Gateway.DI
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
        public static IServiceCollection RegisterHttpGatewayCommunicators(this IServiceCollection services)
        {
            services.RegisterHttpServiceCommunicator();

            services.AddSingleton<AuthorizationCommunicator>();
            services.AddSingleton<HRCommunicator>();
            
            return services;
        }
    }
}
