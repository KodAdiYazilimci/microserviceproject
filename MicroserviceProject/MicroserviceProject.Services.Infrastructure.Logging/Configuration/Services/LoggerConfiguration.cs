using MicroserviceProject.Services.Infrastructure.Logging.Util.Logging.Loggers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Services
{
    /// <summary>
    /// Loglayıcı DI sınıfı
    /// </summary>
    public static class LoggerConfiguration
    {
        /// <summary>
        /// Loglayıcıları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="configuration">Configuration nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<RequestResponseLogger>(x => new RequestResponseLogger(configuration));

            return services;
        }
    }
}
