
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers._DI;
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
            services.AddHttpClient();

            services.AddSingleton<HttpGetCaller>();
            services.AddSingleton<HttpPostCaller>();

            services.RegisterCredentialProvider();
            services.RegisterHttpRouteRepositories();
            services.RegisterRoutingProviders();
            services.RegisterInMemoryCaching();

            services.AddSingleton<ServiceCommunicator>();

            return services;
        }
    }
}
