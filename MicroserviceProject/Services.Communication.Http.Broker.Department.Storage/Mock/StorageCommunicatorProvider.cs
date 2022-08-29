
using Infrastructure.Communication.Http.Broker;

namespace Services.Communication.Http.Broker.Department.Storage.Mock
{
    public class StorageCommunicatorProvider
    {
        private static StorageCommunicator storageCommunicator;

        public static StorageCommunicator GetStorageCommunicator(ServiceCommunicator serviceCommunicator)
        {
            if (storageCommunicator == null)
            {
                storageCommunicator = new StorageCommunicator(serviceCommunicator);
            }

            return storageCommunicator;
        }
    }
}
