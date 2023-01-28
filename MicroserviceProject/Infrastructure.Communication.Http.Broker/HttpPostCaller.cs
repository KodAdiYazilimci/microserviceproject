using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Exceptions;

using Newtonsoft.Json;

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

            if (endpoint.Headers.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new MissingHeaderException();
            }

            GenerateHeaders(httpClient, endpoint.Headers);

            if (endpoint.Queries.Any(x => string.IsNullOrEmpty(x.Value)))
            {
                throw new MissingQueryStringException();
            }

            string url = GenerateQueryString(endpoint.Url, endpoint.Queries);

            HttpResponseMessage getTask =
                await httpClient.PostAsync(
                    url,
                    new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json"),
                    cancellationTokenSource.Token);

            return JsonConvert.DeserializeObject<TResult>(await getTask.Content.ReadAsStringAsync(cancellationTokenSource.Token));
        }
    }
}
