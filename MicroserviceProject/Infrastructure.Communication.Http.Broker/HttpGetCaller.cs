using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Helpers;

using Newtonsoft.Json;

using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpGetCaller
    {
        public async Task<TResult> CallAsync<TResult>(IEndpoint endpoint, CancellationTokenSource cancellationTokenSource)
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

                if (authenticatedEndpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
                {
                    throw new MissingQueryStringException($"Belirtilmemiş query: {authenticatedEndpoint.Queries.FirstOrDefault(x => string.IsNullOrEmpty(x.Value)).Name}");
                }

                string url = HttpHelper.GenerateQueryString(authenticatedEndpoint.Url, authenticatedEndpoint.Queries);

                HttpResponseMessage getTask = await httpClient.GetAsync(url, cancellationTokenSource.Token);

                string result = await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token);

                return JsonConvert.DeserializeObject<TResult>(result);
            }
        }
    }
}
