using Services.Communication.Http.Broker.Department.Storage.Models;

using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker;
using Services.Communication.Http.Broker.Department.Storage;

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
