using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.CR.Abstract;

namespace Services.Communication.Http.Broker.Department.CR.Mock
{
    public class CRCommunicatorProvider
    {
        private static ICRCommunicator crCommunicator;

        public static ICRCommunicator GetCRCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (crCommunicator == null)
            {
                crCommunicator = new CRCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return crCommunicator;
        }
    }
}
