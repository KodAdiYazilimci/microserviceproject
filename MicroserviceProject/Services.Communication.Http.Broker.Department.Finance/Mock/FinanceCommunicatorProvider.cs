using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Department.Finance.Mock
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
