using Services.Communication.Http.Broker.Department.HR;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class HRCommunicatorProvider
    {
        private static HRCommunicator hRCommunicator;

        public static HRCommunicator GetHRCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (hRCommunicator == null)
            {
                hRCommunicator = new HRCommunicator(routeNameProvider, serviceCommunicator);
            }

            return hRCommunicator;
        }
    }
}
