using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.IT.Mock
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
