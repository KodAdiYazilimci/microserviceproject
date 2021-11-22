using Services.Communication.Http.Broker.Department.Buying;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
