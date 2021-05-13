using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Communication.WebSockets.DI
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
        public static IServiceCollection RegisterSocketConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<SocketListener>();

            return services;
        }
    }
}
