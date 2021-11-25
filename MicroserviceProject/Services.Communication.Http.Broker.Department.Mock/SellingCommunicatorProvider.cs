using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Selling;

namespace Services.Communication.Http.Broker.Department.Mock
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
