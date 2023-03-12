using Infrastructure.Caching.Abstraction;
using Infrastructure.Routing.Persistence.Abstract;
using Infrastructure.Routing.Providers.Abstract;

namespace Infrastructure.Routing.Providers.Mock
{
    public class RouteProviderFactory
    {
        private static IRouteProvider routeProvider = null;

        public static IRouteProvider GetRouteProvider(
            IServiceRouteRepository serviceRouteRepository,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            if (routeProvider == null)
            {
                routeProvider = new SqlRouteProvider(serviceRouteRepository, inMemoryCacheDataProvider);
            }

            return routeProvider;
        }
    }
}
