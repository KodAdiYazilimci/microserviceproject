using System.Net.Http;

namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpClientFactory
    {
        private static HttpClient httpClient;

        public static HttpClient Instance
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }

                return httpClient;
            }
        }
    }
}
