
using Microsoft.Extensions.DependencyInjection;

namespace Services.Logging.RequestResponse.DI
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
        public static IServiceCollection RegisterRequestResponseLogger(this IServiceCollection services)
        {
            services.AddSingleton<RequestResponseLogger>();

            return services;
        }
    }
}
