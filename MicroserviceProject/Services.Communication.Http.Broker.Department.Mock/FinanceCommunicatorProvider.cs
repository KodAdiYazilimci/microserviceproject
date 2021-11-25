using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Finance;

namespace Services.Communication.Http.Broker.Department.Mock
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
