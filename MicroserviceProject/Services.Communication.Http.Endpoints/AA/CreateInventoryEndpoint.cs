using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstraction;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.Repositories.Sql;

namespace Services.Communication.Http.Endpoints.AA
{
    public class CreateInventoryEndpoint : IHttpEndpoint
    {
        private const string CachePrefix = "SERVICEROUTES_";
        private const string EndpointName = "aa.inventory.createinventory";

        private readonly ServiceRouteRepository _serviceRouteRepository;
        private readonly InMemoryCacheDataProvider _inMemoryCacheDataProvider;

        private ServiceRouteModel? serviceRoute;

        private ServiceRouteModel? ServiceRoute
        {
            get
            {
                if (serviceRoute == null)
                {
                    if (_inMemoryCacheDataProvider.TryGetValue<ServiceRouteModel>(CachePrefix + EndpointName, out ServiceRouteModel _serviceRoute))
                    {
                        if (_serviceRoute != null)
                        {
                            serviceRoute = _serviceRoute;
                        }
                    }
                    else
                    {
                        Task<ServiceRouteModel> routeTask = _serviceRouteRepository.GetServiceRouteAsync(EndpointName, new CancellationTokenSource());

                        routeTask.Wait();

                        serviceRoute = routeTask.Result;

                        _inMemoryCacheDataProvider.Set<ServiceRouteModel>(CachePrefix + EndpointName, serviceRoute);
                    }
                }

                return serviceRoute;
            }
        }

        public CreateInventoryEndpoint(
            ServiceRouteRepository serviceRouteRepository,
            InMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            _serviceRouteRepository = serviceRouteRepository;
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
        }

        public string Name
        {
            get
            {
                return ServiceRoute.ServiceName;
            }
        }

        public string Url
        {
            get
            {
                return ServiceRoute.Endpoint;
            }
        }

        public HttpVerb HttpVerb
        {
            get { return ServiceRoute.CallType == "POST" ? HttpVerb.POST : HttpVerb.GET; }
        }

        public List<HttpQuery> Queries
        {
            get
            {
                return ServiceRoute.QueryKeys.Select(x => new HttpQuery() { Key = x.Key }).ToList();
            }
        }

        public List<HttpHeader> Headers
        {
            get
            {
                //TODO
                throw new NotImplementedException();
            }
        }
    }
}
