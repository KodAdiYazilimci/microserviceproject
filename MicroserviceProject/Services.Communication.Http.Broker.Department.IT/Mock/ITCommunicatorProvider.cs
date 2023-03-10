using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.IT.Abstract;

namespace Services.Communication.Http.Broker.Department.IT.Mock
{
    public class ITCommunicatorProvider
    {
        private static IITCommunicator itCommunicator;

        public static IITCommunicator GetITCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (itCommunicator == null)
            {
                itCommunicator = new ITCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return itCommunicator;
        }
    }
}
