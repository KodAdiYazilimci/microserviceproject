using Infrastructure.Routing.Persistence.Repositories.Sql;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class ServiceRouteRepositoryFactory
    {
        private static ServiceRouteRepository serviceRouteRepository = null;

        public static ServiceRouteRepository GetServiceRouteRepository(IConfiguration configuration)
        {
            if (serviceRouteRepository == null)
            {
                serviceRouteRepository = new ServiceRouteRepository(configuration);
            }

            return serviceRouteRepository;
        }
    }
}
