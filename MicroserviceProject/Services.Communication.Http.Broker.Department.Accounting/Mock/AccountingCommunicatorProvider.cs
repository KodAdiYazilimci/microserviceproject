using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.Abstract;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
{
    public class AccountingCommunicatorProvider
    {
        private static AccountingCommunicator accountingCommunicator = null;

        public static IAccountingCommunicator GetAccountingCommunicator(
            IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (accountingCommunicator == null)
            {
                accountingCommunicator = new AccountingCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return accountingCommunicator;
        }
    }
}
