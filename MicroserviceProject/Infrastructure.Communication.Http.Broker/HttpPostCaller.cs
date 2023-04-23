using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Util;
using Infrastructure.Communication.Http.Helpers;

using Newtonsoft.Json;

using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker
{
    public class HttpPostCaller
    {
        public async Task<TResult> CallAsync<TRequest, TResult>(IAuthenticatedEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpHelper.GenerateHeaders(httpClient, endpoint.Headers);

                endpoint.EndpointAuthentication.SetAuthentication(httpClient);

                string url = HttpHelper.GenerateQueryString(endpoint.Url, endpoint.Queries);

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
