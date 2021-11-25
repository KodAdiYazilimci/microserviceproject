
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Communication.Http.Broker.DI
{
    /// <summary>
    /// Servis iletişim sağlayıcı DI sınıfı
    /// </summary>
    public static class ServiceCommunicationConfiguration
    {
        /// <summary>
        /// Servis iletişim sağlayıcı sınıfı enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterHttpServiceCommunicator(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterHttpRouteProvider();
            services.RegisterHttpRouteRepositories();

            services.AddSingleton<ServiceCommunicator>();

            return services;
        }
    }
}
