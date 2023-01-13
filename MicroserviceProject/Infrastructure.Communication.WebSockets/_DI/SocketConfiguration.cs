using Infrastructure.Caching.Abstraction;
using Infrastructure.Caching.InMemory;
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Security.Authentication.DI;
using Infrastructure.Sockets.Persistence.Repositories.Sql;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading;
using System.Linq;

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
            services.RegisterHttpRouteRepositories();

            services.AddSingleton<ServiceCaller>(service =>
            {
                var httpClient = service.GetRequiredService<HttpClient>();
                var inMemoryCacheDataProvider = service.GetRequiredService<InMemoryCacheDataProvider>();
                var serviceRouteRepository = service.GetRequiredService<ServiceRouteRepository>();
                var instance = new ServiceCaller(httpClient, inMemoryCacheDataProvider);

                instance.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    List<ServiceRouteModel> serviceRoutes = inMemoryCacheDataProvider.Get<List<ServiceRouteModel>>(SocketListener.CACHEDSERVICEROUTES);

                    if (serviceRoutes == null || !serviceRoutes.Any())
                    {
                        serviceRoutes = await serviceRouteRepository.GetServiceRoutesAsync(new CancellationTokenSource());

                        inMemoryCacheDataProvider.Set<List<ServiceRouteModel>>(SocketListener.CACHEDSERVICEROUTES, serviceRoutes, DateTime.UtcNow.AddMinutes(60));
                    }

                    if (serviceRoutes.Any(x => x.ServiceName == serviceName))
                        return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
                    else
                        throw new GetRouteException("Servis rotası bulunamadı");
                };

                return instance;
            });
            services.AddSingleton<SocketListener>();
            services.AddSingleton<SocketRepository>();

            return services;
        }
    }
}
