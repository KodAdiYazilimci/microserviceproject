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
using Infrastructure.ServiceDiscovery.Exceptions;
using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Discoverer.Discovers
{
    public class HttpServiceDiscoverer : IServiceDiscoverer
    {
        private const string CACHED_SERVICE_NAME_PREFIX = "CACHED_SERVICE_NAME_";
        private readonly ISolidServiceProvider _solidServiceProvider;
        private readonly ISolidServiceConfiguration _solidServiceConfiguration;
        private readonly HttpGetCaller _httpGetCaller;
        private readonly IInMemoryCacheDataProvider _inMemoryCacheDataProvider;

        public HttpServiceDiscoverer(
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            HttpGetCaller httpGetCaller,
            ISolidServiceProvider solidServiceProvider,
            ISolidServiceConfiguration solidServiceConfiguration)
        {
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
            _httpGetCaller = httpGetCaller;
            _solidServiceProvider = solidServiceProvider;
            _solidServiceConfiguration = solidServiceConfiguration;
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
                SolidServiceModel solidService = _solidServiceProvider.GetSolidService();

                if (solidService != null)
                {
                    ServiceResultModel<ServiceModel> serviceResult = await _httpGetCaller.CallAsync<ServiceResultModel<ServiceModel>>(new DiscoverEndpoint()
                    {
                        Url = solidService.Address,
                        HttpAction = HttpAction.GET,
                        EndpointAuthentication = new AnonymouseAuthentication(),
                        Queries = new Dictionary<string, string>(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("ServiceName", serviceName)
                        })
                    }, cancellationTokenSource);

                    if (serviceResult != null && serviceResult.IsSuccess && serviceResult.Data != null)
                    {
                        DateTime validTo = DateTime.Now.AddMinutes(_solidServiceConfiguration.ExpirationServiceInfo);

                        return new CachedServiceModel()
                        {
                            ValidTo = validTo,
                            Endpoints = serviceResult.Data.Endpoints,
                            Port = serviceResult.Data.Port,
                            Protocol = serviceResult.Data.Protocol,
                            ServiceName = serviceResult.Data.ServiceName
                        };
                    }
                    else
                        throw new SolidServiceCouldtNotFetchException(serviceResult?.ErrorModel?.Description ?? "Solid service couldn't fetch");
                }
                else
                    throw new SolidServiceNotDefinedException();
            }
        }
    }
}
