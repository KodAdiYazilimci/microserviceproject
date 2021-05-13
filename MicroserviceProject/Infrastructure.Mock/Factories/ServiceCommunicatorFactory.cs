using Infrastructure.Communication.Moderator;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Providers;

using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Mock.Factories
{
    public class ServiceCommunicatorFactory
    {
        private static ServiceCommunicator serviceCommunicator = null;

        public static ServiceCommunicator GetServiceCommunicator(
            IMemoryCache memoryCache,
            CredentialProvider credentialProvider,
            RouteNameProvider routeNameProvider,
            ServiceRouteRepository serviceRouteRepository)
        {
            if (serviceCommunicator == null)
            {
                serviceCommunicator = new ServiceCommunicator(memoryCache, credentialProvider, routeNameProvider, serviceRouteRepository);
            }

            return serviceCommunicator;
        }
    }
}
