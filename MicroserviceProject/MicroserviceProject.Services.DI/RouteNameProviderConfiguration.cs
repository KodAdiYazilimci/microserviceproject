using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.DI
{
    /// <summary>
    /// Servis rotaları DI sınıfı
    /// </summary>
    public static class RouteNameProviderConfiguration
    {
        /// <summary>
        /// Servis rotalarını enjekte eder
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
