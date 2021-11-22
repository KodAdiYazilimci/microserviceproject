using Services.Communication.Http.Broker.Department.AA;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
