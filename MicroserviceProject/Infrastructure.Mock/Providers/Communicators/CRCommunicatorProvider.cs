using Services.Communication.Http.Broker.Department.CR;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
