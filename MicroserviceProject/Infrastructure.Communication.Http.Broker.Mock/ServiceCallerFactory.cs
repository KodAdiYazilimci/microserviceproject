using Infrastructure.Caching.InMemory;

using System.Net.Http;

namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class ServiceCallerFactory
    {
        private static ServiceCaller serviceCaller = null;

        public static ServiceCaller GetServiceCaller(
            HttpClient httpClient,
            InMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            if (serviceCaller == null)
            {
                serviceCaller = new ServiceCaller(httpClient, inMemoryCacheDataProvider);
            }

            return serviceCaller;
        }
    }
}
