using Communication.Http.Department.Production;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class ProductionCommunicatorProvider
    {
        private static ProductionCommunicator productionCommunicator;

        public static ProductionCommunicator GetProductionCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (productionCommunicator == null)
            {
                productionCommunicator = new ProductionCommunicator(routeNameProvider, serviceCommunicator);
            }

            return productionCommunicator;
        }
    }
}
