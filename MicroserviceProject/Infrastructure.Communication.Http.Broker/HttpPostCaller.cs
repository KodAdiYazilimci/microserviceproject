using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Helpers;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpPostCaller
    {
        public async Task<TResult> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                IAuthenticatedEndpoint authenticatedEndpoint = endpoint.ConvertToAuthenticatedEndpoint();

                authenticatedEndpoint.EndpointAuthentication.SetAuthentication(httpClient);

                if (authenticatedEndpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)))
                {
                    throw new MissingHeaderException($"Belirtilmemiş header: {authenticatedEndpoint.Headers.FirstOrDefault(x => string.IsNullOrEmpty(x.Value)).Name}");
                }

                HttpHelper.GenerateHeaders(httpClient, endpoint.Headers);

                if (endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
                {
                    throw new MissingQueryStringException($"Belirtilmemiş query: {authenticatedEndpoint.Queries.FirstOrDefault(x => string.IsNullOrEmpty(x.Value)).Name}");
                }

                string url = HttpHelper.GenerateQueryString(authenticatedEndpoint.Url, authenticatedEndpoint.Queries);

                HttpResponseMessage getTask =
                    await httpClient.PostAsync(
                        url,
                        new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json"),
                        cancellationTokenSource.Token);

                return JsonConvert.DeserializeObject<TResult>(await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token));
            }
        }
    }
}
