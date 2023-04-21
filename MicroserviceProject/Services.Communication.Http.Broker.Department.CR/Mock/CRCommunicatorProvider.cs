using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.CR.Abstract;

namespace Services.Communication.Http.Broker.Department.CR.Mock
{
    public class CRCommunicatorProvider
    {
        private static ICRCommunicator crCommunicator;

        public static ICRCommunicator GetCRCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (crCommunicator == null)
            {
                crCommunicator = new CRCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return crCommunicator;
        }
    }
}
