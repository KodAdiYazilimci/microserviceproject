
using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Infrastructure.Security.Model;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Communication.Moderator
{
    /// <summary>
    /// Yetki denetimi destekli servis iletişim sağlayıcı sınıf
    /// </summary>
    public class ServiceCommunicator
    {
        /// <summary>
        /// Çağrıda kullanılacak yetki tokenının önbellekteki adı
        /// </summary>
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        /// <summary>
        /// Servis endpointlerinin önbellekteki adı
        /// </summary>
        private const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";

        /// <summary>
        /// Önbellek nesnesi
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// İletişimde kullanılacak yetkiler için sağlayıcı
        /// </summary>
        private readonly CredentialProvider _credentialProvider;

        /// <summary>
        /// Gerektiğinde iletişimde bulunacak yetki servisi için rota isimleri sağlayıcısı
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Servis endpointleri sağlayıcısı
        /// </summary>
        private readonly ServiceRouteRepository _serviceRouteRepository;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıf
        /// </summary>
        /// <param name="memoryCache">Önbellek nesnesi</param>
        /// <param name="credentialProvider">İletişimde kullanılacak yetkiler için sağlayıcı</param>
        /// <param name="routeNameProvider">Gerektiğinde iletişimde bulunacak yetki servisi için rota isimleri sağlayıcısı</param>
        /// <param name="serviceRouteRepository">Servis endpointleri sağlayıcısı</param>
        public ServiceCommunicator(
            IMemoryCache memoryCache,
            CredentialProvider credentialProvider,
            RouteNameProvider routeNameProvider,
            ServiceRouteRepository serviceRouteRepository)
        {
            _memoryCache = memoryCache;
            _credentialProvider = credentialProvider;
            _routeNameProvider = routeNameProvider;
            _serviceRouteRepository = serviceRouteRepository;
        }

        /// <summary>
        /// Yetki denetimi altında servis çağrısı oluşturur
        /// </summary>
        /// <typeparam name="T">Çağrıdan dönmesi beklenen tip</typeparam>
        /// <param name="serviceName">Çağrı yapılacak servisin adı</param>
        /// <param name="postData">Çağrı yapılacak servise post edilecek payload data</param>
        /// <param name="queryParameters">Çağrı yapılacak servis gönderilecek query string parametreler</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResult<T>> Call<T>(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            CancellationToken cancellationToken)
        {
            Token takenTokenForThisService = _memoryCache.Get<Token>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationToken);
                };
                ServiceResult<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: _routeNameProvider.Auth_GetToken,
                        postData: new Credential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        cancellationToken: cancellationToken);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _memoryCache.Set<Token>(TAKENTOKENFORTHISSERVICE, tokenResult.Data);
                }
                else
                {
                    throw new Exception("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            ServiceCaller serviceCaller = new ServiceCaller(_memoryCache, takenTokenForThisService.TokenKey);
            serviceCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
            {
                return await GetServiceAsync(serviceName, cancellationToken);
            };

            ServiceResult<T> result = await serviceCaller.Call<T>(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// Yetki denetimi altında servis çağrısı oluşturur
        /// </summary>
        /// <param name="serviceName">Çağrı yapılacak servisin adı</param>
        /// <param name="postData">Çağrı yapılacak servise post edilecek payload data</param>
        /// <param name="queryParameters">Çağrı yapılacak servis gönderilecek query string parametreler</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResult> Call(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            CancellationToken cancellationToken)
        {
            Token takenTokenForThisService = _memoryCache.Get<Token>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationToken);
                };
                ServiceResult<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: _routeNameProvider.Auth_GetToken,
                        postData: new Credential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        cancellationToken: cancellationToken);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _memoryCache.Set<Token>(TAKENTOKENFORTHISSERVICE, tokenResult.Data);
                }
                else
                {
                    throw new Exception("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            ServiceCaller serviceCaller = new ServiceCaller(_memoryCache, takenTokenForThisService.TokenKey);
            serviceCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
            {
                return await GetServiceAsync(serviceName, cancellationToken);
            };

            ServiceResult result = await serviceCaller.Call(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                cancellationToken: cancellationToken);

            return result;
        }

        /// <summary>
        /// Servis rota bilgisini verir
        /// </summary>
        /// <param name="serviceName">Bilgisi getirilecek servisin adı</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        private async Task<string> GetServiceAsync(string serviceName, CancellationToken cancellationToken)
        {
            List<ServiceRoute> serviceRoutes = _memoryCache.Get<List<ServiceRoute>>(CACHEDSERVICEROUTES);

            if (serviceRoutes == null || !serviceRoutes.Any())
            {
                serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationToken);

                return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
            }

            serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationToken);

            _memoryCache.Set<List<ServiceRoute>>(CACHEDSERVICEROUTES, serviceRoutes, DateTime.Now.AddMinutes(60));

            return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
        }
    }
}
