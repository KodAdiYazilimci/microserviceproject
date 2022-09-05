using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.Selling.Mock
{
    public class SellingCommunicatorProvider
    {
        private static SellingCommunicator sellingCommunicator;

        public static SellingCommunicator GetSellingCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (sellingCommunicator == null)
            {
                sellingCommunicator = new SellingCommunicator(serviceCommunicator);
            }

            return sellingCommunicator;
        }
    }
}
