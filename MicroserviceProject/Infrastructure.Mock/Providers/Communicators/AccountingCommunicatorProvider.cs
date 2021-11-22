using Services.Communication.Http.Broker.Department.Accounting;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;

namespace Infrastructure.Mock.Providers.Communicators
{
    public class AccountingCommunicatorProvider
    {
        private static AccountingCommunicator accountingCommunicator;

        public static AccountingCommunicator GetAccountingCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (accountingCommunicator == null)
            {
                accountingCommunicator = new AccountingCommunicator(routeNameProvider, serviceCommunicator);
            }

            return accountingCommunicator;
        }
    }
}
