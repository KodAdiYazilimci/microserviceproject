
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.DI
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
        public static IServiceCollection RegisterHttpRouteProvider(this IServiceCollection services)
        {
            services.AddSingleton<RouteNameProvider>();

            return services;
        }
    }
}
