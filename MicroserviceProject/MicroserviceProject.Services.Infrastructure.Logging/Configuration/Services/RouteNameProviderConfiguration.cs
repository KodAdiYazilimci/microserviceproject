using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Services
{
    /// <summary>
    /// Servis endpoint isimleri DI sınıfı
    /// </summary>
    public static class RouteNameProviderConfiguration
    {
        /// <summary>
        /// Servis endpoint isimlerini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="configuration">Configuration nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRouteProvider(this IServiceCollection services)
        {
            services.AddSingleton<RouteNameProvider>();

            return services;
        }
    }
}
