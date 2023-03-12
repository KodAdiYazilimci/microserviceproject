using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.HR.Abstract;

namespace Services.Communication.Http.Broker.Department.HR.Mock
{
    public class HRCommunicatorProvider
    {
        private static IHRCommunicator hRCommunicator;

        public static IHRCommunicator GetHRCommunicator(
            IRouteProvider routeProvider,
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
