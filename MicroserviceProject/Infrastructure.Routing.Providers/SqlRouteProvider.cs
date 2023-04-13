using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Persistence.Abstract;
using Infrastructure.Routing.Providers.Abstract;

namespace Infrastructure.Routing.Providers
{
    public class SqlRouteProvider : IRouteProvider
    {
        private const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";
        private readonly IServiceRouteRepository _serviceRouteRepository;
        private readonly IInMemoryCacheDataProvider _inMemoryCacheDataProvider;

        public SqlRouteProvider(
            IServiceRouteRepository serviceRouteRepository,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            _serviceRouteRepository = serviceRouteRepository;
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
        }

        public async Task<IEndpoint> GetRoutingEndpointAsync<TEndpoint>(CancellationTokenSource cancellationTokenSource) where TEndpoint : IEndpoint, new()
        {
            IEndpoint endpointInstance = Activator.CreateInstance<TEndpoint>();

            if (_inMemoryCacheDataProvider.TryGetValue(CACHEDSERVICEROUTES, out List<IEndpoint> endpoints)
                &&
                endpoints.Any(x => x.Name == endpointInstance.Name))
            {
                return endpoints.Where(x => x.Name == endpointInstance.Name).FirstOrDefault();
            }
            else
            {
                if (endpoints == null)
                    endpoints = new List<IEndpoint>();

                var route = await _serviceRouteRepository.GetServiceRouteAsync(endpointInstance.Name, cancellationTokenSource);

                if (route != null)
                {
                    endpointInstance.Url = route.Endpoint;
                    endpointInstance.HttpAction = route.CallType == "GET" ? HttpAction.GET : HttpAction.POST;
                    endpointInstance.Queries = route.QueryKeys.Select(x => new HttpQueryModel() { Name = x.Key }).ToList();
                    endpointInstance.Headers = new List<HttpHeaderModel>();

                    endpoints.Add(endpointInstance);

                    _inMemoryCacheDataProvider.Set<List<IEndpoint>>(CACHEDSERVICEROUTES, endpoints);

                    return endpointInstance;
                }
                throw new GetRouteException();
            }
        }
    }
}
