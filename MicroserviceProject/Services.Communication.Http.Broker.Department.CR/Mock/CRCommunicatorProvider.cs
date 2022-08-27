using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.CR.Mock
{
    public class CRCommunicatorProvider
    {
        private static CRCommunicator crCommunicator;

        public static CRCommunicator GetCRCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (crCommunicator == null)
            {
                crCommunicator = new CRCommunicator(routeNameProvider, serviceCommunicator);
            }

            return crCommunicator;
        }
    }
}
