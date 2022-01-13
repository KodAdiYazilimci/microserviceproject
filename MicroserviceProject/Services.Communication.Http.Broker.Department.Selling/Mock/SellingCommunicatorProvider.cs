using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Department.Selling.Mock
{
    public class SellingCommunicatorProvider
    {
        private static SellingCommunicator sellingCommunicator;

        public static SellingCommunicator GetSellingCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (sellingCommunicator == null)
            {
                sellingCommunicator = new SellingCommunicator(routeNameProvider, serviceCommunicator);
            }

            return sellingCommunicator;
        }
    }
}
