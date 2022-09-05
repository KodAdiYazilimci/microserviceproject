using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.AA.Mock
{
    public class AACommunicatorProvider
    {
        private static AACommunicator aaCommunicator;

        public static AACommunicator GetAACommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (aaCommunicator == null)
            {
                aaCommunicator = new AACommunicator(serviceCommunicator);
            }

            return aaCommunicator;
        }
    }
}
