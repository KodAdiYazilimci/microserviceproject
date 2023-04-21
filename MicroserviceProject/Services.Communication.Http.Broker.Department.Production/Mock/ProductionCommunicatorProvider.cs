using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Production.Abstract;

namespace Services.Communication.Http.Broker.Department.Production.Mock
{
    public class ProductionCommunicatorProvider
    {
        private static IProductionCommunicator productionCommunicator;

        public static IProductionCommunicator GetProductionCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (productionCommunicator == null)
            {
                productionCommunicator = new ProductionCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return productionCommunicator;
        }
    }
}
