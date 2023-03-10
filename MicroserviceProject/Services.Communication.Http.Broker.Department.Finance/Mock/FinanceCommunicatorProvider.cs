using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Finance.Abstract;

namespace Services.Communication.Http.Broker.Department.Finance.Mock
{
    public class FinanceCommunicatorProvider
    {
        private static IFinanceCommunicator financeCommunicator;

        public static IFinanceCommunicator GetFinanceCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (financeCommunicator == null)
            {
                financeCommunicator = new FinanceCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return financeCommunicator;
        }
    }
}
