using MicroserviceProject.Infrastructure.Communication.Http.Models;
using MicroserviceProject.Infrastructure.Communication.Http.Providers;
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Moderator.Models;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Communication.Moderator
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
        /// Servis bilgisi önbellekte bulunamadı tutucu
        /// </summary>
        /// <param name="serviceName">Bulunamayan servis adı</param>
        /// <returns></returns>
        public delegate Task<string> NoServiceFoundInCacheHandlerAsync(string serviceName);

        /// <summary>
        /// Servis bilgisi önbellekte bulunamadığında ateşlenecek olay.
        /// Dönüş değeri yeniden önbelleğe yüklenir
        /// </summary>
        public event NoServiceFoundInCacheHandlerAsync OnNoServiceFoundInCacheAsync;

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
            ServiceRoute serviceRoute = null;

            try
            {
                string serviceJson = _memoryCache.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

                if (string.IsNullOrEmpty(serviceJson))
                {
                    if (OnNoServiceFoundInCacheAsync != null)
                    {
                        serviceJson = await OnNoServiceFoundInCacheAsync(serviceName);
                    }

                    _memoryCache.Set<string>(
                        key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                        value: serviceJson,
                        absoluteExpiration: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
                }

                if (!string.IsNullOrEmpty(serviceJson))
                {
                    serviceRoute = JsonConvert.DeserializeObject<ServiceRoute>(serviceJson);

                    if (serviceRoute != null)
                    {
                        if (!string.IsNullOrEmpty(serviceRoute.CallType))
                        {
                            if (serviceRoute.CallType.ToUpper() == "POST")
                            {
                                HttpPostProvider httpPostProvider = new HttpPostProvider();
                                httpPostProvider.Headers = new List<HttpHeader>();
                                httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                                SetQueryParameters(queryParameters, serviceRoute, httpPostProvider);

                                return
                                    await
                                    httpPostProvider
                                    .PostAsync<ServiceResult, object>(
                                        url: serviceRoute.Endpoint,
                                        postData: postData,
                                        cancellationToken: cancellationToken);
                            }
                            else if (serviceRoute.CallType.ToUpper() == "GET")
                            {
                                HttpGetProvider httpGetProvider = new HttpGetProvider();
                                httpGetProvider.Headers = new List<HttpHeader>();
                                httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                                SetQueryParameters(queryParameters, serviceRoute, httpGetProvider);

                                return
                                    await
                                    httpGetProvider
                                    .GetAsync<ServiceResult>(
                                        url: serviceRoute.Endpoint, cancellationToken);
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
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null)
                {
                    if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new ServiceResult() { IsSuccess = false, Error = new Error() { Code = "401", Description = wex.ToString() } };
                    }
                    else if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.BadRequest)
                    {
                        using (StreamReader streamReader = new StreamReader(wex.Response.GetResponseStream()))
                        {
                            string response = await streamReader.ReadToEndAsync();

                            return JsonConvert.DeserializeObject<ServiceResult>(response);
                        }
                    }
                    else if (wex.Response is HttpWebResponse)
                    {
                        if (serviceRoute != null && serviceRoute.AlternativeRoutes != null && serviceRoute.AlternativeRoutes.Any())
                        {
                            foreach (var route in serviceRoute.AlternativeRoutes)
                            {
                                var alternativeCallResult = await Call(
                                    serviceName: route.ServiceName,
                                    postData: postData,
                                    queryParameters: queryParameters,
                                    cancellationToken: cancellationToken);

                                if (alternativeCallResult.IsSuccess)
                                    return alternativeCallResult;
                            }
                        }
                    }
                }
                else if (wex.Status == WebExceptionStatus.UnknownError)
                {
                    if (serviceRoute != null && serviceRoute.AlternativeRoutes != null && serviceRoute.AlternativeRoutes.Any())
                    {
                        foreach (var route in serviceRoute.AlternativeRoutes)
                        {
                            var alternativeCallResult = await Call(
                                serviceName: route.ServiceName,
                                postData: postData,
                                queryParameters: queryParameters,
                                cancellationToken: cancellationToken);

                            if (alternativeCallResult.IsSuccess)
                                return alternativeCallResult;
                        }
                    }
                }

                return new ServiceResult() { IsSuccess = false, Error = new Error() { Description = wex.ToString() } };
            }
            catch (Exception ex)
            {
                return new ServiceResult() { IsSuccess = false, Error = new Error() { Description = ex.ToString() } };
            }
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
            ServiceRoute serviceRoute = null;

            try
            {
                string serviceJson = _memoryCache.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

                if (string.IsNullOrEmpty(serviceJson))
                {
                    if (OnNoServiceFoundInCacheAsync != null)
                    {
                        serviceJson = await OnNoServiceFoundInCacheAsync(serviceName);
                    }

                    _memoryCache.Set<string>(
                        key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                        value: serviceJson,
                        absoluteExpiration: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
                }

                if (!string.IsNullOrEmpty(serviceJson))
                {
                    serviceRoute = JsonConvert.DeserializeObject<ServiceRoute>(serviceJson);

                    if (serviceRoute != null)
                    {
                        if (!string.IsNullOrEmpty(serviceRoute.CallType))
                        {
                            if (serviceRoute.CallType.ToUpper() == "POST")
                            {
                                HttpPostProvider httpPostProvider = new HttpPostProvider();
                                httpPostProvider.Headers = new List<HttpHeader>();
                                httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                                SetQueryParameters(queryParameters, serviceRoute, httpPostProvider);

                                return
                                    await
                                    httpPostProvider
                                    .PostAsync<ServiceResult<TResult>, object>(
                                        url: serviceRoute.Endpoint,
                                        postData: postData,
                                        cancellationToken: cancellationToken);
                            }
                            else if (serviceRoute.CallType.ToUpper() == "GET")
                            {
                                HttpGetProvider httpGetProvider = new HttpGetProvider();
                                httpGetProvider.Headers = new List<HttpHeader>();
                                httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                                SetQueryParameters(queryParameters, serviceRoute, httpGetProvider);

                                return
                                    await
                                    httpGetProvider
                                    .GetAsync<ServiceResult<TResult>>(
                                        url: serviceRoute.Endpoint, cancellationToken);
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
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null)
                {
                    if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new ServiceResult<TResult>() { IsSuccess = false, Error = new Error() { Code = "401", Description = wex.ToString() } };
                    }
                    else if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.BadRequest)
                    {
                        using (StreamReader streamReader = new StreamReader(wex.Response.GetResponseStream()))
                        {
                            string response = await streamReader.ReadToEndAsync();

                            return JsonConvert.DeserializeObject<ServiceResult<TResult>>(response);
                        }
                    }
                    else if (wex.Response is HttpWebResponse)
                    {
                        if (serviceRoute != null && serviceRoute.AlternativeRoutes != null && serviceRoute.AlternativeRoutes.Any())
                        {
                            foreach (var route in serviceRoute.AlternativeRoutes)
                            {
                                var alternativeCallResult = await Call<TResult>(
                                    serviceName: route.ServiceName,
                                    postData: postData,
                                    queryParameters: queryParameters,
                                    cancellationToken: cancellationToken);

                                if (alternativeCallResult.IsSuccess)
                                    return alternativeCallResult;
                            }
                        }
                    }
                }
                else if (wex.Status == WebExceptionStatus.UnknownError)
                {
                    if (serviceRoute != null && serviceRoute.AlternativeRoutes != null && serviceRoute.AlternativeRoutes.Any())
                    {
                        foreach (var route in serviceRoute.AlternativeRoutes)
                        {
                            var alternativeCallResult = await Call<TResult>(
                                serviceName: route.ServiceName,
                                postData: postData,
                                queryParameters: queryParameters,
                                cancellationToken: cancellationToken);

                            if (alternativeCallResult.IsSuccess)
                                return alternativeCallResult;
                        }
                    }
                }

                return new ServiceResult<TResult>() { IsSuccess = false, Error = new Error() { Description = wex.ToString() } };
            }
            catch (Exception ex)
            {
                return new ServiceResult<TResult>() { IsSuccess = false, Error = new Error() { Description = ex.ToString() } };
            }
        }

        /// <summary>
        /// Servis çağrısına gerekirse query string parametreleri ekler
        /// </summary>
        /// <param name="queryParameters">Eklenecek query string parametreleri</param>
        /// <param name="callModel">Servisin çağrı modeli</param>
        /// <param name="httpGetProvider">HttpGet sağlayıcısı</param>
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, ServiceRoute callModel, HttpGetProvider httpGetProvider)
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
                    if (queryParameters.Any(x => x.Key == key.Key)
                        &&
                        !string.IsNullOrEmpty(queryParameters.FirstOrDefault(x => x.Key == key.Key).Value))
                    {
                        httpGetProvider.Queries.Add(new HttpQuery()
                        {
                            Key = key.Key,
                            Value = queryParameters.FirstOrDefault(x => x.Key == key.Key).Value
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
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, ServiceRoute callModel, HttpPostProvider httpPostProvider)
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
                    if (queryParameters.Any(x => x.Key == key.Key)
                        &&
                        !string.IsNullOrEmpty(queryParameters.FirstOrDefault(x => x.Key == key.Key).Value))
                    {
                        httpPostProvider.Queries.Add(new HttpQuery()
                        {
                            Key = key.Key,
                            Value = queryParameters.FirstOrDefault(x => x.Key == key.Key).Value
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
