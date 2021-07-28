using Communication.Http.Department.Buying;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class BuyingCommunicatorProvider
    {
        private static BuyingCommunicator buyingCommunicator;

        public static BuyingCommunicator GetBuyingCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (buyingCommunicator == null)
            {
                buyingCommunicator = new BuyingCommunicator(routeNameProvider, serviceCommunicator);
            }

            return buyingCommunicator;
        }
    }
}
