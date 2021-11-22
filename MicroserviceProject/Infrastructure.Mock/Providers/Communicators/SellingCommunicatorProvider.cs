using Services.Communication.Http.Broker.Department.Selling;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

namespace Infrastructure.Mock.Providers.Communicators
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
