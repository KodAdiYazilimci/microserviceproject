using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Providers;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    /// <summary>
    /// Bir servisle çağrı kurmayı sağlayan moderatör sınıf
    /// </summary>
    public class ServiceCaller : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
        private readonly InMemoryCacheDataProvider _cacheProvider;

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
        /// <param name="cacheProvider">Servis bilgisini tutan önbellek nesnesi</param>
        /// <param name="serviceToken">Kurulacak servisin beklediği token</param>
        public ServiceCaller(
            InMemoryCacheDataProvider cacheProvider,
            string serviceToken)
        {
            _cacheProvider = cacheProvider;
            _serviceToken = serviceToken;
        }

        /// <summary>
        /// Servis çağrısı kurar
        /// </summary>
        /// <param name="serviceName">İletişime geçilecek servisin adı</param>
        /// <param name="postData">Gerektiğinde servise post edilecek veri</param>
        /// <param name="queryParameters">Gerektiğinde servise verilecek query string parametreleri</param>
        /// <param name="headers">Gerektiğinde servise verilecek Http header parametreleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> Call(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceRouteModel serviceRoute = null;
            ErrorModel errorModel = new ErrorModel();

            try
            {
                string serviceJson = _cacheProvider.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

                if (string.IsNullOrEmpty(serviceJson))
                {
                    if (OnNoServiceFoundInCacheAsync != null)
                    {
                        serviceJson = await OnNoServiceFoundInCacheAsync(serviceName);
                    }

                    _cacheProvider.Set<string>(
                        key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                        value: serviceJson,
                        toTime: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
                }

                if (!string.IsNullOrEmpty(serviceJson))
                {
                    serviceRoute = JsonConvert.DeserializeObject<ServiceRouteModel>(serviceJson);

                    while (serviceRoute != null)
                    {
                        try
                        {
                            return await ExecuteHttpCall(serviceRoute, postData, queryParameters, headers, cancellationTokenSource);
                        }
                        catch (WebException wex)
                        {
                            if (wex.Response != null)
                            {
                                if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    errorModel.InnerErrors.Add(new ErrorModel()
                                    {
                                        Code = "401",
                                        Description = wex.Message
                                    });

                                    return new ServiceResultModel()
                                    {
                                        IsSuccess = false,
                                        SourceApiService = serviceRoute.Endpoint,
                                        ErrorModel = errorModel
                                    };
                                }
                                else if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.BadRequest)
                                {
                                    using (StreamReader streamReader = new StreamReader(wex.Response.GetResponseStream()))
                                    {
                                        string response = await streamReader.ReadToEndAsync();

                                        return JsonConvert.DeserializeObject<ServiceResultModel>(response);
                                    }
                                }
                            }

                            if (serviceRoute.AlternativeRoute != null)
                            {
                                serviceRoute = serviceRoute.AlternativeRoute;
                            }
                            else
                                serviceRoute = null;    
                        }
                    }
                }

                throw new GetRouteException("Servis çağrısı bulunamadı");
            }
            catch (Exception ex)
            {
                errorModel.Description = ex.ToString();
            }

            return new ServiceResultModel() { IsSuccess = false, SourceApiService = serviceRoute.Endpoint, ErrorModel = errorModel };
        }


        /// <summary>
        /// Servis çağrısı kurar
        /// </summary>
        /// <typeparam name="TResult">Servisten beklenen sonucun tipi</typeparam>
        /// <param name="serviceRoute">İletişime geçilecek servisin adı</param>
        /// <param name="postData">Gerektiğinde servise post edilecek veri</param>
        /// <param name="queryParameters">Gerektiğinde servise verilecek query string parametreleri</param>
        /// <param name="headers">Gerektiğinde servise verilecek Http header parametreleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<TResult>> Call<TResult>(
            string serviceName,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceRouteModel serviceRoute = null;
            ErrorModel errorModel = new ErrorModel();

            try
            {
                string serviceJson = _cacheProvider.Get<string>(SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower());

                if (string.IsNullOrEmpty(serviceJson))
                {
                    if (OnNoServiceFoundInCacheAsync != null)
                    {
                        serviceJson = await OnNoServiceFoundInCacheAsync(serviceName);
                    }

                    _cacheProvider.Set<string>(
                        key: SERVICE_ENDPOINT_CACHE_PREFIX + serviceName.ToLower(),
                        value: serviceJson,
                        toTime: DateTime.Now.AddMinutes(SERVICE_ENDPOINT_CACHE_TIMEOUT));
                }

                if (!string.IsNullOrEmpty(serviceJson))
                {
                    serviceRoute = JsonConvert.DeserializeObject<ServiceRouteModel>(serviceJson);

                    while (serviceRoute != null)
                    {
                        try
                        {
                            return await ExecuteHttpCall<TResult>(serviceRoute, postData, queryParameters, headers, cancellationTokenSource);
                        }
                        catch (WebException wex)
                        {
                            if (wex.Response != null)
                            {
                                if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    errorModel.InnerErrors.Add(new ErrorModel()
                                    {
                                        Code = "401",
                                        Description = wex.Message
                                    });

                                    return new ServiceResultModel<TResult>()
                                    {
                                        IsSuccess = false,
                                        SourceApiService = serviceRoute.Endpoint,
                                        ErrorModel = errorModel
                                    };
                                }
                                else if (wex.Response is HttpWebResponse && (wex.Response as HttpWebResponse).StatusCode == HttpStatusCode.BadRequest)
                                {
                                    using (StreamReader streamReader = new StreamReader(wex.Response.GetResponseStream()))
                                    {
                                        string response = await streamReader.ReadToEndAsync();

                                        return JsonConvert.DeserializeObject<ServiceResultModel<TResult>>(response);
                                    }
                                }
                            }

                            if (serviceRoute.AlternativeRoute != null)
                            {
                                serviceRoute = serviceRoute.AlternativeRoute;
                            }
                            else
                                serviceRoute = null;
                        }
                    }
                }

                throw new GetRouteException("Servis çağrısı bulunamadı");
            }
            catch (Exception ex)
            {
                errorModel.Description = ex.ToString();
            }

            return new ServiceResultModel<TResult>() { IsSuccess = false, SourceApiService = serviceRoute.Endpoint, ErrorModel = errorModel };
        }

        private async Task<ServiceResultModel> ExecuteHttpCall(
            ServiceRouteModel serviceRoute,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            if (!string.IsNullOrEmpty(serviceRoute.CallType))
            {
                if (serviceRoute.CallType.ToUpper() == "POST")
                {
                    HttpPostProvider httpPostProvider = new HttpPostProvider
                    {
                        Headers = new List<HttpHeader>()
                    };

                    httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                    if (headers != null)
                    {
                        foreach (var httpHeader in headers)
                        {
                            httpPostProvider.Headers.Add(new HttpHeader(httpHeader.Key, httpHeader.Value));
                        }
                    }

                    SetQueryParameters(queryParameters, serviceRoute, httpPostProvider);

                    return
                        await
                        httpPostProvider
                        .PostAsync<ServiceResultModel, object>(
                            url: serviceRoute.Endpoint,
                            postData: postData,
                            cancellationTokenSource: cancellationTokenSource);
                }
                else if (serviceRoute.CallType.ToUpper() == "GET")
                {
                    HttpGetProvider httpGetProvider = new HttpGetProvider
                    {
                        Headers = new List<HttpHeader>()
                    };

                    httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                    if (headers != null)
                    {
                        foreach (var httpHeader in headers)
                        {
                            httpGetProvider.Headers.Add(new HttpHeader(httpHeader.Key, httpHeader.Value));
                        }
                    }

                    SetQueryParameters(queryParameters, serviceRoute, httpGetProvider);

                    return
                        await
                        httpGetProvider
                        .GetAsync<ServiceResultModel>(
                            url: serviceRoute.Endpoint, cancellationTokenSource);
                }
                else
                {
                    throw new WrongCallTypeException("Servis çağrı tipi doğru belirtilmemiş");
                }
            }
            else
            {
                throw new UndefinedCallTypeException("Servis çağrı tipi belirtilmemiş");
            }
        }

        private async Task<ServiceResultModel<TResult>> ExecuteHttpCall<TResult>(
            ServiceRouteModel serviceRoute,
            object postData,
            List<KeyValuePair<string, string>> queryParameters,
            List<KeyValuePair<string, string>> headers,
            CancellationTokenSource cancellationTokenSource)
        {
            if (!string.IsNullOrEmpty(serviceRoute.CallType))
            {
                if (serviceRoute.CallType.ToUpper() == "POST")
                {
                    HttpPostProvider httpPostProvider = new HttpPostProvider
                    {
                        Headers = new List<HttpHeader>()
                    };

                    httpPostProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                    if (headers != null)
                    {
                        foreach (var httpHeader in headers)
                        {
                            httpPostProvider.Headers.Add(new HttpHeader(httpHeader.Key, httpHeader.Value));
                        }
                    }

                    SetQueryParameters(queryParameters, serviceRoute, httpPostProvider);

                    return
                        await
                        httpPostProvider
                        .PostAsync<ServiceResultModel<TResult>, object>(
                            url: serviceRoute.Endpoint,
                            postData: postData,
                            cancellationTokenSource: cancellationTokenSource);
                }
                else if (serviceRoute.CallType.ToUpper() == "GET")
                {
                    HttpGetProvider httpGetProvider = new HttpGetProvider
                    {
                        Headers = new List<HttpHeader>()
                    };

                    httpGetProvider.Headers.Add(new HttpHeader("Authorization", _serviceToken));

                    if (headers != null)
                    {
                        foreach (var httpHeader in headers)
                        {
                            httpGetProvider.Headers.Add(new HttpHeader(httpHeader.Key, httpHeader.Value));
                        }
                    }

                    SetQueryParameters(queryParameters, serviceRoute, httpGetProvider);

                    return
                        await
                        httpGetProvider
                        .GetAsync<ServiceResultModel<TResult>>(
                            url: serviceRoute.Endpoint, cancellationTokenSource);
                }
                else
                {
                    throw new WrongCallTypeException("Servis çağrı tipi doğru belirtilmemiş");
                }
            }
            else
            {
                throw new UndefinedCallTypeException("Servis çağrı tipi belirtilmemiş");
            }
        }

        /// <summary>
        /// Servis çağrısına gerekirse query string parametreleri ekler
        /// </summary>
        /// <param name="queryParameters">Eklenecek query string parametreleri</param>
        /// <param name="callModel">Servisin çağrı modeli</param>
        /// <param name="httpGetProvider">HttpGet sağlayıcısı</param>
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, ServiceRouteModel callModel, HttpGetProvider httpGetProvider)
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
        private void SetQueryParameters(List<KeyValuePair<string, string>> queryParameters, ServiceRouteModel callModel, HttpPostProvider httpPostProvider)
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

                }

                disposed = true;
            }
        }
    }
}
