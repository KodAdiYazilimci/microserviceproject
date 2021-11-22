using Services.Communication.Http.Broker.Department.Production;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
