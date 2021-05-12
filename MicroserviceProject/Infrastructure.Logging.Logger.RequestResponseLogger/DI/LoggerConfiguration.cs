
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Infrastructure.Logging.Logger.RequestResponseLogger.DI
{
    /// <summary>
    /// Loggerların DI sınıfı
    /// </summary>
    public static class LoggerConfiguration
    {
        /// <summary>
        /// Loggerları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterLogger(this IServiceCollection services)
        {
            services.AddSingleton<Logger>();

            return services;
        }
    }
}
