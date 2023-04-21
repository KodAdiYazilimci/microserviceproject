using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Selling.Abstract;

namespace Services.Communication.Http.Broker.Department.Selling.Mock
{
    public class SellingCommunicatorProvider
    {
        private static ISellingCommunicator sellingCommunicator;

        public static ISellingCommunicator GetSellingCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (sellingCommunicator == null)
            {
                sellingCommunicator = new SellingCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return sellingCommunicator;
        }
    }
}
