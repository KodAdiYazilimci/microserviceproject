using Infrastructure.Caching.InMemory;
using Infrastructure.Routing.Persistence.Abstract;
using Infrastructure.Routing.Providers.Abstract;

namespace Infrastructure.Routing.Providers.Mock
{
    public class RouteProviderFactory
    {
        private static IRouteProvider routeProvider = null;

        public static IRouteProvider GetRouteProvider(
            IServiceRouteRepository serviceRouteRepository,
            InMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            if (routeProvider == null)
            {
                routeProvider = new SqlRouteProvider(serviceRouteRepository, inMemoryCacheDataProvider);
            }

            return routeProvider;
        }
    }
}
