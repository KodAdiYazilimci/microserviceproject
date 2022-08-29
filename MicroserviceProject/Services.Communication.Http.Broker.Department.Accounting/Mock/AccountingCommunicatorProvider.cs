using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
{
    public class AccountingCommunicatorProvider
    {
        private static AccountingCommunicator accountingCommunicator;

        public static AccountingCommunicator GetAccountingCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (accountingCommunicator == null)
            {
                accountingCommunicator = new AccountingCommunicator(serviceCommunicator);
            }

            return accountingCommunicator;
        }
    }
}
