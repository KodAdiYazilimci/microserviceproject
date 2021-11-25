
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Storage;

namespace Services.Communication.Http.Broker.Department.Mock
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
