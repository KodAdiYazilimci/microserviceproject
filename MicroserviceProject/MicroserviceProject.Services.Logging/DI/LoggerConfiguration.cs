using MicroserviceProject.Services.Logging.Loggers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Logging.DI
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
            services.AddSingleton<RequestResponseLogger>();

            return services;
        }
    }
}
