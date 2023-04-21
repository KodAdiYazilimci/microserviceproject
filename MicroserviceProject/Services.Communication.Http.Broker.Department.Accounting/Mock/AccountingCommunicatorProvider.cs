using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.Abstract;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
{
    public class AccountingCommunicatorProvider
    {
        private static AccountingCommunicator accountingCommunicator = null;

        public static IAccountingCommunicator GetAccountingCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (accountingCommunicator == null)
            {
                accountingCommunicator = new AccountingCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return accountingCommunicator;
        }
    }
}
