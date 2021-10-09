using Communication.Http.Department.CR;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
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
