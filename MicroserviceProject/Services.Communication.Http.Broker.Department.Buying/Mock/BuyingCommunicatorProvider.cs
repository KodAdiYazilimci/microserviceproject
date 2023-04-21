using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Buying.Abstract;

namespace Services.Communication.Http.Broker.Department.Buying.Mock
{
    public class BuyingCommunicatorProvider
    {
        private static IBuyingCommunicator buyingCommunicator;

        public static IBuyingCommunicator GetBuyingCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (buyingCommunicator == null)
            {
                buyingCommunicator = new BuyingCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return buyingCommunicator;
        }
    }
}
