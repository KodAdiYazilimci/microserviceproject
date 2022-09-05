using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.CR.Mock
{
    public class CRCommunicatorProvider
    {
        private static CRCommunicator crCommunicator;

        public static CRCommunicator GetCRCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (crCommunicator == null)
            {
                crCommunicator = new CRCommunicator(serviceCommunicator);
            }

            return crCommunicator;
        }
    }
}
