using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.Buying.Mock
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
