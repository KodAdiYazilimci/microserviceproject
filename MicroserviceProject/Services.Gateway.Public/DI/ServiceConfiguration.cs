
using Microsoft.Extensions.DependencyInjection;

using Services.Gateway.Public.Services;

namespace Services.Gateway.Public.DI
{
    /// <summary>
    /// Servislerin DI sınıfı
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Servisleri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<HRService>();

            return services;
        }
    }
}
