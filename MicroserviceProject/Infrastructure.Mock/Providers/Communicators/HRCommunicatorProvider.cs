using Communication.Http.Department.HR;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

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
