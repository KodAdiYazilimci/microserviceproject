using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

namespace Services.WebSockets.DI
{
    /// <summary>
    /// Web soketlerin DI sınıfı
    /// </summary>
    public static class SocketConfiguration
    {
        /// <summary>
        /// Web soketlerini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSocketListeners(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();

            services.AddSingleton<SocketListener>();

            return services;
        }
    }
}
