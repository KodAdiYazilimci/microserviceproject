using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
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
