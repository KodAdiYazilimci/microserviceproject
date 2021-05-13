
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Sockets.Providers.DI
{
    /// <summary>
    /// Servis rotaları DI sınıfı
    /// </summary>
    public static class SocketNameProviderConfiguration
    {
        /// <summary>
        /// Servis rotalarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="configuration">Configuration nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRouteProvider(this IServiceCollection services)
        {
            services.AddSingleton<SocketNameProvider>();

            return services;
        }
    }
}
