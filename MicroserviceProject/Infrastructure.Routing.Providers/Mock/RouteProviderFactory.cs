using Infrastructure.Caching.InMemory;
using Infrastructure.Routing.Persistence.Repositories.Sql;

namespace Infrastructure.Routing.Providers.Mock
{
    public class RouteProviderFactory
    {
        private static RouteProvider routeProvider = null;

        public static RouteProvider GetRouteProvider(
            ServiceRouteRepository serviceRouteRepository,
            InMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            if (routeProvider == null)
            {
                routeProvider = new RouteProvider(serviceRouteRepository, inMemoryCacheDataProvider);
            }

            return routeProvider;
        }
    }
}
