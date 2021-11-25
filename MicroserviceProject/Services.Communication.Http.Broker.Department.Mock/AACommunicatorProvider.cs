using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.AA;

namespace Services.Communication.Http.Broker.Department.Mock
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
