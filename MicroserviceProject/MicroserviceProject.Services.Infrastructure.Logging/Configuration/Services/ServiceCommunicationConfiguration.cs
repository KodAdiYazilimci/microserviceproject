using MicroserviceProject.Infrastructure.Communication.Moderator;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Services
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
