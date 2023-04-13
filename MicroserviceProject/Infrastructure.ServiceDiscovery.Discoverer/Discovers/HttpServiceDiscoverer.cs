using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Endpoints;
using Infrastructure.ServiceDiscovery.Discoverer.Exceptions;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

namespace Infrastructure.ServiceDiscovery.Discoverer.Discovers
{
    public class HttpServiceDiscoverer : IServiceDiscoverer
    {
        private const string CACHED_SERVICE_NAME_PREFIX = "CACHED_SERVICE_NAME_";
        private readonly ISolidServiceConfiguration _solidServiceConfiguration;
        private readonly IDiscoveryConfiguration _discoveryConfiguration;
        private readonly HttpGetCaller _httpGetCaller;
        private readonly IInMemoryCacheDataProvider _inMemoryCacheDataProvider;

        public HttpServiceDiscoverer(
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            HttpGetCaller httpGetCaller,
            ISolidServiceConfiguration solidServiceConfiguration,
            IDiscoveryConfiguration discoveryConfiguration)
        {
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
            _httpGetCaller = httpGetCaller;
            _solidServiceConfiguration = solidServiceConfiguration;
            _discoveryConfiguration = discoveryConfiguration;
        }

        public async Task<CachedServiceModel> GetServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource)
        {
            if (_inMemoryCacheDataProvider.TryGetValue<CachedServiceModel>(CACHED_SERVICE_NAME_PREFIX + serviceName, out CachedServiceModel _cachedServiceModel)
                &&
                _cachedServiceModel != null)
            {
                return _cachedServiceModel;
            }
            else
            {
                ServiceResultModel<DiscoveredServiceModel> serviceResult = await _httpGetCaller.CallAsync<ServiceResultModel<DiscoveredServiceModel>>(new DiscoverEndpoint()
                {
                    Url = _solidServiceConfiguration.DiscoverAddress,
                    HttpAction = HttpAction.GET,
                    EndpointAuthentication = new AnonymouseAuthentication(),
                    Queries = new List<HttpQueryModel>()
                        {
                            new HttpQueryModel(){ Name = "ServiceName", Value = serviceName}
                        }
                }, cancellationTokenSource);

                if (serviceResult != null && serviceResult.IsSuccess && serviceResult.Data != null)
                {
                    DateTime validTo = DateTime.Now.AddMinutes(_discoveryConfiguration.ExpirationServiceInfo);

                    return new CachedServiceModel()
                    {
                        ValidTo = validTo,
                        Endpoints = serviceResult.Data.Endpoints,
                        Port = serviceResult.Data.Port,
                        Protocol = serviceResult.Data.Protocol,
                        ServiceName = serviceResult.Data.ServiceName,
                        DnsName = serviceResult.Data.DnsName,
                        IpAddresses = serviceResult.Data.IpAddresses,
                    };
                }
                else
                    throw new SolidServiceCouldtNotFetchException(serviceResult?.ErrorModel?.Description ?? "Solid service couldn't fetch");
            }
        }
    }
}
