using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Production;

namespace Services.Communication.Http.Broker.Department.Mock
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
