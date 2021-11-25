using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Accounting;

namespace Services.Communication.Http.Broker.Department.Mock
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
