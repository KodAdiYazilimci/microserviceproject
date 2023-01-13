using Infrastructure.Caching.InMemory;
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

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

            services.RegisterCredentialProvider();
            services.RegisterHttpRouteRepositories();
            services.RegisterInMemoryCaching();

            services.AddSingleton<ServiceCaller>(service =>
            {
                var httpClient = service.GetRequiredService<IHttpClientFactory>();
                var inMemoryCacheDataProvider = service.GetRequiredService<InMemoryCacheDataProvider>();
                var serviceRouteRepository = service.GetRequiredService<ServiceRouteRepository>();

                var instance = new ServiceCaller(httpClient.CreateClient(), inMemoryCacheDataProvider);

                instance.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    List<ServiceRouteModel> serviceRoutes = inMemoryCacheDataProvider.Get<List<ServiceRouteModel>>(ServiceCommunicator.CACHEDSERVICEROUTES);

                    if (serviceRoutes == null || !serviceRoutes.Any())
                    {
                        serviceRoutes = await serviceRouteRepository.GetServiceRoutesAsync(new CancellationTokenSource());

                        inMemoryCacheDataProvider.Set<List<ServiceRouteModel>>(ServiceCommunicator.CACHEDSERVICEROUTES, serviceRoutes, DateTime.UtcNow.AddMinutes(60));
                    }

                    return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
                };

                return instance;
            });

            services.AddSingleton<ServiceCommunicator>();

            return services;
        }
    }
}
