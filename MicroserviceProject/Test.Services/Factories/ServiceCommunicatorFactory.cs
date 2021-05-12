using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Routing.Persistence.Repositories.Sql;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Infrastructure.Security.Providers;

using Microsoft.Extensions.Caching.Memory;

namespace MicroserviceProject.Test.Services.Factories
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
