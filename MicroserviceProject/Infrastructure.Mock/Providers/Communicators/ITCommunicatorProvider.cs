using Communication.Http.Department.IT;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class ITCommunicatorProvider
    {
        private static ITCommunicator itCommunicator;

        public static ITCommunicator GetITCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (itCommunicator == null)
            {
                itCommunicator = new ITCommunicator(routeNameProvider, serviceCommunicator);
            }

            return itCommunicator;
        }
    }
}
