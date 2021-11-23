
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Sockets.Providers.DI
{
    /// <summary>
    /// Soket isim sağlayıcıları DI sınıfı
    /// </summary>
    public static class SocketNameProviderConfiguration
    {
        /// <summary>
        /// Soket isim sağlayıcılarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSocketRouteNameProvider(this IServiceCollection services)
        {
            services.AddSingleton<SocketNameProvider>();

            return services;
        }
    }
}
