﻿using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Authentication.Providers;
using Infrastructure.Security.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
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
        public const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        /// <summary>
        /// Servis endpointlerinin önbellekteki adı
        /// </summary>
        public const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";

        /// <summary>
        /// Önbellek nesnesi
        /// </summary>
        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// Servis endpointleri sağlayıcısı
        /// </summary>
        private readonly ServiceRouteRepository _serviceRouteRepository;

        private readonly ServiceCaller _serviceCaller;

        /// <summary>
        /// İletişimde kullanılacak yetkiler için sağlayıcı
        /// </summary>
        private readonly CredentialProvider _credentialProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıf
        /// </summary>
        /// <param name="cacheProvider">Önbellek nesnesi</param>
        /// <param name="credentialProvider">İletişimde kullanılacak yetkiler için sağlayıcı</param>
        public ServiceCommunicator(
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            ServiceRouteRepository serviceRouteRepository,
            ServiceCaller serviceCaller)
        {
            _cacheProvider = cacheProvider;
            _credentialProvider = credentialProvider;
            _serviceRouteRepository = serviceRouteRepository;
            _serviceCaller = serviceCaller;
        }

        /// <summary>
        /// Yetki denetimi altında servis çağrısı oluşturur
        /// </summary>
        /// <typeparam name="T">Çağrıdan dönmesi beklenen tip</typeparam>
        /// <param name="serviceName">Çağrı yapılacak servisin adı</param>
        /// <param name="postData">Çağrı yapılacak servise post edilecek payload data</param>
        /// <param name="queryParameters">Çağrı yapılacak servis gönderilecek query string parametreler</param>
        /// <param name="headers">Çağrı yapılacak servise gönderilecek Http header parametreleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<T>> Call<T>(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            AuthenticationToken takenTokenForThisService = _cacheProvider.Get<AuthenticationToken>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.UtcNow)
            {
                ServiceResultModel<AuthenticationToken> tokenResult =
                    await _serviceCaller.Call<AuthenticationToken>(
                        serviceName: "authorization.auth.gettoken",
                        postData: new AuthenticationCredential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        headers: null,
                        serviceToken: string.Empty,
                        cancellationTokenSource: cancellationTokenSource);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _cacheProvider.Set<AuthenticationToken>(TAKENTOKENFORTHISSERVICE, tokenResult.Data, tokenResult.Data.ValidTo.AddMinutes(-1));
                }
                else
                {
                    throw new GetTokenException("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            ServiceResultModel<T> result = await _serviceCaller.Call<T>(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                headers: headers,
                serviceToken: takenTokenForThisService.TokenKey,
                cancellationTokenSource: cancellationTokenSource);

            return result;
        }

        /// <summary>
        /// Yetki denetimi altında servis çağrısı oluşturur
        /// </summary>
        /// <param name="serviceName">Çağrı yapılacak servisin adı</param>
        /// <param name="postData">Çağrı yapılacak servise post edilecek payload data</param>
        /// <param name="queryParameters">Çağrı yapılacak servis gönderilecek query string parametreler</param>
        /// <param name="headers">Çağrı yapılacak servise gönderilecek Http header parametreleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> Call(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            AuthenticationToken takenTokenForThisService = _cacheProvider.Get<AuthenticationToken>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.UtcNow)
            {
                ServiceResultModel<AuthenticationToken> tokenResult =
                    await _serviceCaller.Call<AuthenticationToken>(
                        serviceName: "authorization.auth.gettoken",
                        postData: new AuthenticationCredential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        headers: null,
                        serviceToken: string.Empty,
                        cancellationTokenSource: cancellationTokenSource);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _cacheProvider.Set<AuthenticationToken>(TAKENTOKENFORTHISSERVICE, tokenResult.Data, tokenResult.Data.ValidTo.AddMinutes(-1));
                }
                else
                {
                    throw new GetTokenException("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            ServiceResultModel result = await _serviceCaller.Call(
                serviceName: serviceName,
                postData: postData,
                queryParameters: queryParameters,
                headers: headers,
                serviceToken: takenTokenForThisService.TokenKey,
                cancellationTokenSource: cancellationTokenSource);

            return result;
        }

        /// <summary>
        /// Servis rota bilgisini verir
        /// </summary>
        /// <param name="serviceName">Bilgisi getirilecek servisin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<string> GetServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> serviceRoutes = _cacheProvider.Get<List<ServiceRouteModel>>(CACHEDSERVICEROUTES);

            if (serviceRoutes == null || !serviceRoutes.Any())
            {
                serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationTokenSource);

                _cacheProvider.Set<List<ServiceRouteModel>>(CACHEDSERVICEROUTES, serviceRoutes, DateTime.UtcNow.AddMinutes(60));
            }

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
                }

                disposed = true;
            }
        }
    }
}
