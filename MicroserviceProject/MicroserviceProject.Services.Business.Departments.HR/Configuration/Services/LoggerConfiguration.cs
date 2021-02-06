using MicroserviceProject.Services.Business.Departments.HR.Util.Logging.Loggers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Services
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
