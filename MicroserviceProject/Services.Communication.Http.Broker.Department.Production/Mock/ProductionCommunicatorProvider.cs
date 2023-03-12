using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Production.Abstract;

namespace Services.Communication.Http.Broker.Department.Production.Mock
{
    public class ProductionCommunicatorProvider
    {
        private static IProductionCommunicator productionCommunicator;

        public static IProductionCommunicator GetProductionCommunicator(
            IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (productionCommunicator == null)
            {
                productionCommunicator = new ProductionCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return productionCommunicator;
        }
    }
}
