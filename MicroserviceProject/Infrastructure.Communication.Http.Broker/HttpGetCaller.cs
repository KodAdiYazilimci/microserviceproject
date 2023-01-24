using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Http.Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpGetCaller : BaseCaller
    {
        public async Task<TResult> CallAsync<TResult>(IEndpoint endpoint, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            endpoint.EndpointAuthentication.SetAuthentication(httpClient);

            if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)) || endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            HttpResponseMessage getTask = await httpClient.GetAsync(endpoint.Url, cancellationTokenSource.Token);

            return JsonConvert.DeserializeObject<TResult>(await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token));
        }

        public async Task<TResult> CallAsync<TResult>(IEndpoint endpoint, List<HttpHeader> headers, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            GenerateHeaders(httpClient, headers);

            if (endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            return await CallAsync<TResult>(endpoint, cancellationTokenSource);
        }

        public async Task<TResult> CallAsync<TResult>(IEndpoint endpoint, List<HttpQuery> httpQueries, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            GenerateQueryString(httpClient, endpoint, httpQueries);

            if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new Exception();
            }

            return await CallAsync<TResult>(endpoint, cancellationTokenSource);
        }

        public async Task<TResult> CallAsync<TResult>(IEndpoint endpoint, List<HttpHeader> headers, List<HttpQuery> httpQueries, CancellationTokenSource cancellationTokenSource)
        {
            HttpClient httpClient = new HttpClient();

            headers.ForEach(header =>
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            });

            GenerateQueryString(httpClient, endpoint, httpQueries);

            return await CallAsync<TResult>(endpoint, cancellationTokenSource);
        }
    }
}
