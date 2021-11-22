using Services.Communication.Http.Broker.Department.Finance;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

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
