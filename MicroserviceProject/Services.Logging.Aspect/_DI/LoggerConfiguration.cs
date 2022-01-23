
using Microsoft.Extensions.DependencyInjection;

using Services.Logging.Aspect.Handlers;

namespace Services.Logging.Aspect.DI
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
        public static IServiceCollection RegisterRuntimeLogger(this IServiceCollection services)
        {
            services.AddSingleton<RuntimeLogger>();
            services.AddSingleton<RuntimeLogHandler>();

            return services;
        }
    }
}
