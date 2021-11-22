using Services.Communication.Http.Broker.Department.IT;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
