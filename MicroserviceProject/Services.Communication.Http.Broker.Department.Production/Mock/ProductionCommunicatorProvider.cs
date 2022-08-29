using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.Production.Mock
{
    public class ProductionCommunicatorProvider
    {
        private static ProductionCommunicator productionCommunicator;

        public static ProductionCommunicator GetProductionCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (productionCommunicator == null)
            {
                productionCommunicator = new ProductionCommunicator(serviceCommunicator);
            }

            return productionCommunicator;
        }
    }
}
