using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.Abstract;

namespace Services.Communication.Http.Broker.Department.AA.Mock
{
    public class AACommunicatorProvider
    {
        private static AACommunicator aaCommunicator;

        public static IAACommunicator GetAACommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (aaCommunicator == null)
            {
                aaCommunicator = new AACommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return aaCommunicator;
        }
    }
}
