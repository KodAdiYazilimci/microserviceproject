using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Exceptions;
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
            using (HttpClient httpClient = new HttpClient())
            {
                endpoint.EndpointAuthentication.SetAuthentication(httpClient);

                if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)))
                {
                    throw new MissingHeaderException($"Belirtilmemiş header: {endpoint.Headers.FirstOrDefault(x => string.IsNullOrEmpty(x.Value)).Key}");
                }

                GenerateHeaders(httpClient, endpoint.Headers);

                if (endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
                {
                    throw new MissingQueryStringException($"Belirtilmemiş query: {endpoint.Queries.FirstOrDefault(x => string.IsNullOrEmpty(x.Value)).Key}");
                }

                string url = GenerateQueryString(endpoint.Url, endpoint.Queries);

                HttpResponseMessage getTask = await httpClient.GetAsync(url, cancellationTokenSource.Token);

                return JsonConvert.DeserializeObject<TResult>(await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token));
            }
        }
    }
}
