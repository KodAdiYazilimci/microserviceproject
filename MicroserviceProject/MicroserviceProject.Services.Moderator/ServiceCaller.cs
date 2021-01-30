using MicroserviceProject.Infrastructure.Communication.Http.Models;
using MicroserviceProject.Infrastructure.Communication.Http.Providers;
using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Services.Moderator.Models;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Moderator
{
    /// <summary>
    /// Bir servisle çağrı kurmayı sağlayan moderatör sınıf
    /// </summary>
    public class ServiceCaller
    {
        /// <summary>
        /// Servis bilgisinin önbellekte tutulacak isminin ön eki
        /// </summary>
        private const string SERVICE_ENDPOINT_CACHE_PREFIX = "services.endpoints.";

        /// <summary>
        /// Servis bilgisinin önbellekte tutulacak süresinin dakika cinsinden değeri
        /// </summary>
        private const double SERVICE_ENDPOINT_CACHE_TIMEOUT = 60;

        /// <summary>
        /// Servis bilgisini tutan önbellek nesnesi
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Kurulacak servisin beklediği token
        /// </summary>
        private readonly string _serviceToken;

        /// <summary>
        /// Bir servisle çağrı kurmayı sağlayan moderatör sınıf
        /// </summary>
        /// <param name="memoryCache">Servis bilgisini tutan önbellek nesnesi</param>
        /// <param name="serviceToken">Kurulacak servisin beklediği token</param>
        public ServiceCaller(
            IMemoryCache memoryCache,
            string serviceToken)
        {
            _memoryCache = memoryCache;
            _serviceToken = serviceToken;
        }

        /// <summary>
        /// Servis çağrısı kurar
        /// </summary>
        /// <param name="serviceName">İletişime geçilecek servisin adı</param>
        /// <param name="postData">Gerektiğinde servise post edilecek veri</param>
        /// <param name="queryParameters">Gerektiğinde servise verilecek query string parametreleri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResult> Call(
          string serviceName,
          object postData,
          List<KeyValuePair<string, string>> queryParameters,
          CancellationToken cancellationToken)
        {
            string serviceJson = _memoryCache.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

            if (string.IsNullOrEmpty(serviceJson))
            {
                serviceJson = ""; // TO DO: Servis bilgisini tekrar çekmeyi dene

                _memoryCache.Set<string>(
                    key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                    value: serviceJson,
                    absoluteExpiration: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
            }

            if (!string.IsNullOrEmpty(serviceJson))
            {
                CallModel callModel = JsonConvert.DeserializeObject<CallModel>(serviceJson);

                if (callModel != null)
                {
                    if (!string.IsNullOrEmpty(callModel.CallType))
                    {
                        if (callModel.CallType.ToUpper() == "POST")
                        {
                            HttpPostProvider httpPostProvider = new HttpPostProvider();
                            httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                            SetQueryParameters(queryParameters, callModel, httpPostProvider);

                            return
                                await
                                httpPostProvider
                                .PostAsync<ServiceResult, object>(
                                    url: callModel.Endpoint,
                                    postData: postData,
                                    cancellationToken: cancellationToken);
                        }
                        else if (callModel.CallType.ToUpper() == "GET")
                        {
                            HttpGetProvider httpGetProvider = new HttpGetProvider();
                            httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                            SetQueryParameters(queryParameters, callModel, httpGetProvider);

                            return
                                await
                                httpGetProvider
                                .GetAsync<ServiceResult>(
                                    url: callModel.ServiceName, cancellationToken);
                        }
                        else
                        {
                            throw new Exception("Servis çağrı tipi doğru belirtilmemiş");
                        }
                    }
                    else
                    {
                        throw new Exception("Servis çağrı tipi belirtilmemiş");
                    }
                }
            }

            throw new Exception("Servis çağrısı bulunamadı");
        }

        /// <summary>
        /// Servis çağrısı kurar
        /// </summary>
        /// <typeparam name="TResult">Servisten beklenen sonucun tipi</typeparam>
        /// <param name="serviceName">İletişime geçilecek servisin adı</param>
        /// <param name="postData">Gerektiğinde servise post edilecek veri</param>
        /// <param name="queryParameters">Gerektiğinde servise verilecek query string parametreleri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResult<TResult>> Call<TResult>(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            CancellationToken cancellationToken)
        {
            string serviceJson = _memoryCache.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

            if (string.IsNullOrEmpty(serviceJson))
            {
                serviceJson = ""; // TO DO: Servis bilgisini tekrar çekmeyi dene

                _memoryCache.Set<string>(
                    key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                    value: serviceJson,
                    absoluteExpiration: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
            }

            if (!string.IsNullOrEmpty(serviceJson))
            {
                CallModel callModel = JsonConvert.DeserializeObject<CallModel>(serviceJson);

                if (callModel != null)
                {
                    if (!string.IsNullOrEmpty(callModel.CallType))
                    {
                        if (callModel.CallType.ToUpper() == "POST")
                        {
                            HttpPostProvider httpPostProvider = new HttpPostProvider();
                            httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                            SetQueryParameters(queryParameters, callModel, httpPostProvider);

                            return
                                await
                                httpPostProvider
                                .PostAsync<ServiceResult<TResult>, object>(
                                    url: callModel.Endpoint,
                                    postData: postData,
                                    cancellationToken: cancellationToken);
                        }
                        else if (callModel.CallType.ToUpper() == "GET")
                        {
                            HttpGetProvider httpGetProvider = new HttpGetProvider();
                            httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                            SetQueryParameters(queryParameters, callModel, httpGetProvider);

                            return
                                await
                                httpGetProvider
                                .GetAsync<ServiceResult<TResult>>(
                                    url: callModel.ServiceName, cancellationToken);
                        }
                        else
                        {
                            throw new Exception("Servis çağrı tipi doğru belirtilmemiş");
                        }
                    }
                    else
                    {
                        throw new Exception("Servis çağrı tipi belirtilmemiş");
                    }
                }
            }

            throw new Exception("Servis çağrısı bulunamadı");
        }

        /// <summary>
        /// Servis çağrısına gerekirse query string parametreleri ekler
        /// </summary>
        /// <param name="queryParameters">Eklenecek query string parametreleri</param>
        /// <param name="callModel">Servisin çağrı modeli</param>
        /// <param name="httpGetProvider">HttpGet sağlayıcısı</param>
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, CallModel callModel, HttpGetProvider httpGetProvider)
        {
            if (callModel.QueryKeys != null && callModel.QueryKeys.Any())
            {
                if (queryParameters == null || !queryParameters.Any())
                {
                    throw new Exception("Servisin gerektirdiği query string parametreleri eksik");
                }

                httpGetProvider.Queries = new List<HttpQuery>();

                foreach (var key in callModel.QueryKeys)
                {
                    if (queryParameters.Any(x => x.Key == key)
                        &&
                        !string.IsNullOrEmpty(queryParameters.FirstOrDefault(x => x.Key == key).Value))
                    {
                        httpGetProvider.Queries.Add(new HttpQuery()
                        {
                            Key = key,
                            Value = queryParameters.FirstOrDefault(x => x.Key == key).Value
                        });
                    }
                    else
                    {
                        throw new Exception($"Servisin gerektirdiği {key} parametresi eksik");
                    }
                }
            }
        }

        /// <summary>
        /// Servis çağrısına gerekirse query string parametreleri ekler
        /// </summary>
        /// <param name="queryParameters">Eklenecek query string parametreleri</param>
        /// <param name="callModel">Servisin çağrı modeli</param>
        /// <param name="httpPostProvider">HttpPost sağlayıcısı</param>
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, CallModel callModel, HttpPostProvider httpPostProvider)
        {
            if (callModel.QueryKeys != null && callModel.QueryKeys.Any())
            {
                if (queryParameters == null || !queryParameters.Any())
                {
                    throw new Exception("Servisin gerektirdiği query string parametreleri eksik");
                }

                httpPostProvider.Queries = new List<HttpQuery>();

                foreach (var key in callModel.QueryKeys)
                {
                    if (queryParameters.Any(x => x.Key == key)
                        &&
                        !string.IsNullOrEmpty(queryParameters.FirstOrDefault(x => x.Key == key).Value))
                    {
                        httpPostProvider.Queries.Add(new HttpQuery()
                        {
                            Key = key,
                            Value = queryParameters.FirstOrDefault(x => x.Key == key).Value
                        });
                    }
                    else
                    {
                        throw new Exception($"Servisin gerektirdiği {key} parametresi eksik");
                    }
                }
            }
        }
    }
}
