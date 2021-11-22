
using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection RegisterGatewayCommunicators(this IServiceCollection services)
        {
            services.AddSingleton<HRCommunicator>();
            
            return services;
        }
    }
}
