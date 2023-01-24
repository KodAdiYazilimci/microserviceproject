using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;

using Newtonsoft.Json;

using System.Net;

namespace Services.Communication.Http.Broker
{
    public class BaseCommunicator
    {
        private readonly HttpGetCaller _httpGetCaller;
        private readonly HttpPostCaller _httpPostCaller;

        public BaseCommunicator(
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller)
        {
            _httpGetCaller = httpGetCaller;
            _httpPostCaller = httpPostCaller;
        }

        protected async Task<ServiceResultModel<TResult>> CallAsync<TResult>(IEndpoint endpoint, CancellationTokenSource cancellationTokenSource)
        {
            ErrorModel errorModel = new ErrorModel();

            try
            {
                switch (endpoint.HttpAction)
                {
                    case Infrastructure.Communication.Http.Constants.HttpAction.GET:
                        return await _httpGetCaller.CallAsync<ServiceResultModel<TResult>>(endpoint, cancellationTokenSource);
                    default:
                        throw new UndefinedCallTypeException();
                }
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
                            SourceApiService = endpoint.Name,
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
            }
            catch (Exception ex)
            {
                errorModel.Description = ex.ToString();
            }

            return new ServiceResultModel<TResult>() { IsSuccess = false, SourceApiService = endpoint.Name, ErrorModel = errorModel };
        }

        protected async Task<ServiceResultModel<TResult>> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource)
        {
            ErrorModel errorModel = new ErrorModel();

            try
            {
                switch (endpoint.HttpAction)
                {
                    case Infrastructure.Communication.Http.Constants.HttpAction.GET:
                        return await _httpGetCaller.CallAsync<ServiceResultModel<TResult>>(endpoint, cancellationTokenSource);
                    case Infrastructure.Communication.Http.Constants.HttpAction.POST:
                        return await _httpPostCaller.CallAsync<TRequest, ServiceResultModel<TResult>>(endpoint, requestObject, cancellationTokenSource);
                    default:
                        throw new UndefinedCallTypeException();
                }
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
                            SourceApiService = endpoint.Name,
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
            }
            catch (Exception ex)
            {
                errorModel.Description = ex.ToString();
            }

            return new ServiceResultModel<TResult>() { IsSuccess = false, SourceApiService = endpoint.Name, ErrorModel = errorModel };
        }
    }
}
