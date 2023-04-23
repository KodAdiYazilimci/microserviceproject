using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Helpers;

using Newtonsoft.Json;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpGetCaller
    {
        public async Task<TResult> CallAsync<TResult>(IAuthenticatedEndpoint endpoint, CancellationTokenSource cancellationTokenSource)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpHelper.GenerateHeaders(httpClient, endpoint.Headers);

                endpoint.EndpointAuthentication.SetAuthentication(httpClient);

                string url = HttpHelper.GenerateQueryString(endpoint.Url, endpoint.Queries);

                HttpResponseMessage getTask = await httpClient.GetAsync(url, cancellationTokenSource.Token);

                string result = await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token);

                return JsonConvert.DeserializeObject<TResult>(result);
            }
        }
    }
}
