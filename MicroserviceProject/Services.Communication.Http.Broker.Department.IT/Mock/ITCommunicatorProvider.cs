using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.IT.Abstract;

namespace Services.Communication.Http.Broker.Department.IT.Mock
{
    public class ITCommunicatorProvider
    {
        private static IITCommunicator itCommunicator;

        public static IITCommunicator GetITCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (itCommunicator == null)
            {
                itCommunicator = new ITCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return itCommunicator;
        }
    }
}
