using Communication.Http.Department.Storage;

using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;

namespace Infrastructure.Mock.Providers.Communicators
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
