using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.DI;
using Infrastructure.Sockets.Persistence.Repositories.Sql;
using Infrastructure.Sockets.Providers.DI;

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
        public static IServiceCollection RegisterSocketListeners(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterHttpRouteProvider();
            services.RegisterHttpRouteRepositories();
            services.RegisterSocketRouteNameProvider();

            services.AddSingleton<SocketListener>();
            services.AddSingleton<SocketRepository>();

            return services;
        }
    }
}
