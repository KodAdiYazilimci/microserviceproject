using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.HR.Mock
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
