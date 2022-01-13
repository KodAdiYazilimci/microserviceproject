using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Department.Production.Mock
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
