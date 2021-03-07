using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Routing.Model;
using MicroserviceProject.Infrastructure.Routing.Persistence.Repositories.Sql;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Infrastructure.Security.Model;
using MicroserviceProject.Infrastructure.Security.Providers;

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
    public class ServiceCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<T>> Call<T>(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            CancellationTokenSource cancellationTokenSource)
        {
            Token takenTokenForThisService = _memoryCache.Get<Token>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationTokenSource);
                };
                ServiceResultModel<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: _routeNameProvider.Auth_GetToken,
                        postData: new Credential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        cancellationTokenSource: cancellationTokenSource);

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
                return await GetServiceAsync(serviceName, cancellationTokenSource);
            };

            ServiceResultModel<T> result = await serviceCaller.Call<T>(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                cancellationTokenSource: cancellationTokenSource);

            return result;
        }

        /// <summary>
        /// Yetki denetimi altında servis çağrısı oluşturur
        /// </summary>
        /// <param name="serviceName">Çağrı yapılacak servisin adı</param>
        /// <param name="postData">Çağrı yapılacak servise post edilecek payload data</param>
        /// <param name="queryParameters">Çağrı yapılacak servis gönderilecek query string parametreler</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> Call(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            CancellationTokenSource cancellationTokenSource)
        {
            Token takenTokenForThisService = _memoryCache.Get<Token>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationTokenSource);
                };
                ServiceResultModel<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: _routeNameProvider.Auth_GetToken,
                        postData: new Credential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        cancellationTokenSource: cancellationTokenSource);

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
                return await GetServiceAsync(serviceName, cancellationTokenSource);
            };

            ServiceResultModel result = await serviceCaller.Call(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                cancellationTokenSource: cancellationTokenSource);

            return result;
        }

        /// <summary>
        /// Servis rota bilgisini verir
        /// </summary>
        /// <param name="serviceName">Bilgisi getirilecek servisin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        private async Task<string> GetServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> serviceRoutes = _memoryCache.Get<List<ServiceRouteModel>>(CACHEDSERVICEROUTES);

            if (serviceRoutes == null || !serviceRoutes.Any())
            {
                serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationTokenSource);

                return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
            }

            serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationTokenSource);

            _memoryCache.Set<List<ServiceRouteModel>>(CACHEDSERVICEROUTES, serviceRoutes, DateTime.Now.AddMinutes(60));

            return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (_credentialProvider != null)
                        _credentialProvider.Dispose();

                    if (_routeNameProvider != null)
                        _routeNameProvider.Dispose();

                    if (_serviceRouteRepository != null)
                        _serviceRouteRepository.Dispose();
                }

                disposed = true;
            }
        }
    }
}
