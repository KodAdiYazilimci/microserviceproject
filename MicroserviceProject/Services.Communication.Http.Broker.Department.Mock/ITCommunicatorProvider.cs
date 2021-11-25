using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.IT;

namespace Services.Communication.Http.Broker.Department.Mock
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
