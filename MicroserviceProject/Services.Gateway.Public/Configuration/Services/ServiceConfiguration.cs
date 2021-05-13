
using Services.Gateway.Public.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Gateway.Public.Configuration.Services
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
            services.AddScoped<UserService>();
            services.AddScoped<HRService>();

            return services;
        }
    }
}
