using MicroserviceProject.Infrastructure.Routing.Providers;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Test.Services.Factories
{
    public class RouteNameProviderFactory
    {
        private static RouteNameProvider routeNameProvider = null;

        public static RouteNameProvider GetRouteNameProvider(IConfiguration configuration)
        {
            if (routeNameProvider == null)
            {
                routeNameProvider = new RouteNameProvider(configuration);
            }

            return routeNameProvider;
        }
    }
}
