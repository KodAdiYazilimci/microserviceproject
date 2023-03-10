using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.Abstract;

namespace Services.Communication.Http.Broker.Department.Accounting.Mock
{
    public class AccountingCommunicatorProvider
    {
        private static IAccountingCommunicator accountingCommunicator = null;

        public static IAccountingCommunicator GetAccountingCommunicator(
            RouteProvider routeProvider,
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
