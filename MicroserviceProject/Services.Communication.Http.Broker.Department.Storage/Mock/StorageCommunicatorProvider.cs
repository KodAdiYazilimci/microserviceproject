
using Infrastructure.Communication.Http.Broker;

using Services.Communication.Http.Providers;

namespace Services.Communication.Http.Broker.Department.Storage.Mock
{
    public class StorageCommunicatorProvider
    {
        private static StorageCommunicator storageCommunicator;

        public static StorageCommunicator GetStorageCommunicator(RouteNameProvider routeNameProvider, ServiceCommunicator serviceCommunicator)
        {
            if (storageCommunicator == null)
            {
                storageCommunicator = new StorageCommunicator(routeNameProvider, serviceCommunicator);
            }

            return storageCommunicator;
        }
    }
}
