using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpPostCaller : BaseCaller
    {
        public async Task<TResult> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            endpoint.EndpointAuthentication.SetAuthentication(httpClient);

            if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)) || endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            HttpResponseMessage getTask =
                await httpClient.PostAsync(
                    endpoint.Url,
                    new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json"),
                    cancellationTokenSource.Token);

            return JsonConvert.DeserializeObject<TResult>(await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token));
        }

        public async Task<TResult> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, List<HttpHeader> headers, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            GenerateHeaders(httpClient, headers);

            if (endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            return await CallAsync<TRequest, TResult>(endpoint, requestObject, cancellationTokenSource);
        }

        public async Task<TResult> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, List<HttpQuery> httpQueries, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            GenerateQueryString(httpClient, endpoint, httpQueries);

            if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            return await CallAsync<TRequest, TResult>(endpoint, requestObject, cancellationTokenSource);
        }

        public async Task<TResult> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, List<HttpHeader> headers, List<HttpQuery> httpQueries, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            headers.ForEach(header =>
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            });

            GenerateQueryString(httpClient, endpoint, httpQueries);

            return await CallAsync<TRequest, TResult>(endpoint, requestObject, cancellationTokenSource);
        }
    }
}
