using Communication.Http.Department.AA;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class AACommunicatorProvider
    {
        private static AACommunicator aaCommunicator;

        public static AACommunicator GetAACommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (aaCommunicator == null)
            {
                aaCommunicator = new AACommunicator(routeNameProvider, serviceCommunicator);
            }

            return aaCommunicator;
        }
    }
}
