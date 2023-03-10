using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.HR.Abstract;

namespace Services.Communication.Http.Broker.Department.HR.Mock
{
    public class HRCommunicatorProvider
    {
        private static IHRCommunicator hRCommunicator;

        public static IHRCommunicator GetHRCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (hRCommunicator == null)
            {
                hRCommunicator = new HRCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return hRCommunicator;
        }
    }
}
