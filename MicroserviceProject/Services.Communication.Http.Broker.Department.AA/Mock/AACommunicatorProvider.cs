using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Department.AA.Mock
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
