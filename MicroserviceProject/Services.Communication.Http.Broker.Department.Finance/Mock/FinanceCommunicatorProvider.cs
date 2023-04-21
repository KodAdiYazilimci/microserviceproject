using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Finance.Abstract;

namespace Services.Communication.Http.Broker.Department.Finance.Mock
{
    public class FinanceCommunicatorProvider
    {
        private static IFinanceCommunicator financeCommunicator;

        public static IFinanceCommunicator GetFinanceCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (financeCommunicator == null)
            {
                financeCommunicator = new FinanceCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return financeCommunicator;
        }
    }
}
