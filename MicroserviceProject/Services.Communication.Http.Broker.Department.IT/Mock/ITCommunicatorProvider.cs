using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.IT.Mock
{
    public class ITCommunicatorProvider
    {
        private static ITCommunicator itCommunicator;

        public static ITCommunicator GetITCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (itCommunicator == null)
            {
                itCommunicator = new ITCommunicator(serviceCommunicator);
            }

            return itCommunicator;
        }
    }
}
