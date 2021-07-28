using Communication.Http.Department.Finance;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class FinanceCommunicatorProvider
    {
        private static FinanceCommunicator financeCommunicator;

        public static FinanceCommunicator GetFinanceCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (financeCommunicator == null)
            {
                financeCommunicator = new FinanceCommunicator(routeNameProvider, serviceCommunicator);
            }

            return financeCommunicator;
        }
    }
}
