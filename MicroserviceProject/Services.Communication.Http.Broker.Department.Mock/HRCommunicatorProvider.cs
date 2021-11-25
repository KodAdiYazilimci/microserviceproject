using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.HR;

namespace Services.Communication.Http.Broker.Department.Mock
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
