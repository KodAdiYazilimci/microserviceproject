
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Communication.Broker.DI
{
    /// <summary>
    /// Servis iletişim sağlayıcı DI sınıfı
    /// </summary>
    public static class ServiceCommunicationConfiguration
    {
        /// <summary>
        /// Servis iletişim sağlayıcı sınıfı enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterServiceCommunicator(this IServiceCollection services)
        {
            services.AddSingleton<ServiceCommunicator>();

            return services;
        }
    }
}
