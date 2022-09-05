using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.HR.Mock
{
    public class HRCommunicatorProvider
    {
        private static HRCommunicator hRCommunicator;

        public static HRCommunicator GetHRCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (hRCommunicator == null)
            {
                hRCommunicator = new HRCommunicator(serviceCommunicator);
            }

            return hRCommunicator;
        }
    }
}
