using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Discovers;

namespace Infrastructure.ServiceDiscovery.Discoverer.Mock
{
    public static class HttpServiceDiscovererProvider
    {
        public static IServiceDiscoverer GetServiceDiscoverer(
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            HttpGetCaller httpGetCaller,
            ISolidServiceConfiguration solidServiceProvider,
            IDiscoveryConfiguration discoveryConfiguration)
        {
            return new HttpServiceDiscoverer(inMemoryCacheDataProvider, httpGetCaller, solidServiceProvider, discoveryConfiguration);
        }
    }
}
