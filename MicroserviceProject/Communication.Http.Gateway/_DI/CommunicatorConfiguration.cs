
using Communication.Http.Gateway.Public;

using Microsoft.Extensions.DependencyInjection;

namespace Communication.Http.Gateway.DI
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
