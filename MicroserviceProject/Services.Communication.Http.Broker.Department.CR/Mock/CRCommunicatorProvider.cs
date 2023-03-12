using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.CR.Abstract;

namespace Services.Communication.Http.Broker.Department.CR.Mock
{
    public class CRCommunicatorProvider
    {
        private static ICRCommunicator crCommunicator;

        public static ICRCommunicator GetCRCommunicator(
            IRouteProvider routeProvider,
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
