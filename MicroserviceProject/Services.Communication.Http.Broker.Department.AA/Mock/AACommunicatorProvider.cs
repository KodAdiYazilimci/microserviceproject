using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.Abstract;

namespace Services.Communication.Http.Broker.Department.AA.Mock
{
    public class AACommunicatorProvider
    {
        private static AACommunicator aaCommunicator;

        public static IAACommunicator GetAACommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (aaCommunicator == null)
            {
                aaCommunicator = new AACommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return aaCommunicator;
        }
    }
}
