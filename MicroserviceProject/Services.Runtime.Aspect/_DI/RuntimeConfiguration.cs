
using Microsoft.Extensions.DependencyInjection;

using Services.Logging.Aspect.Handlers;

namespace Services.Logging.Aspect.DI
{
    /// <summary>
    /// Çalışma zamanı DI sınıfı
    /// </summary>
    public static class RuntimeConfiguration
    {
        /// <summary>
        /// Çalışma zamanlarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRuntimeHandlers(this IServiceCollection services)
        {
            services.AddScoped<RuntimeLogger>();
            services.AddScoped<RuntimeHandler>();

            return services;
        }
    }
}
