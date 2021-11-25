using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Buying;

namespace Services.Communication.Http.Broker.Department.Mock
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
