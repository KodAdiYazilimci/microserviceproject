using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.HR.Abstract;

namespace Services.Communication.Http.Broker.Department.HR.Mock
{
    public class HRCommunicatorProvider
    {
        private static IHRCommunicator hRCommunicator;

        public static IHRCommunicator GetHRCommunicator(
            //IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (hRCommunicator == null)
            {
                hRCommunicator = new HRCommunicator(
                    departmentCommunicator, serviceDiscoverer);
            }

            return hRCommunicator;
        }
    }
}
