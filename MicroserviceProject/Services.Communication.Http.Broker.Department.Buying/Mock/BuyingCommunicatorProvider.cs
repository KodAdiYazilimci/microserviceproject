using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.Buying.Mock
{
    public class BuyingCommunicatorProvider
    {
        private static BuyingCommunicator buyingCommunicator;

        public static BuyingCommunicator GetBuyingCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (buyingCommunicator == null)
            {
                buyingCommunicator = new BuyingCommunicator(serviceCommunicator);
            }

            return buyingCommunicator;
        }
    }
}
