using Communication.Http.Department.Accounting;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

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
