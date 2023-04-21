using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Storage.Abstract;

namespace Services.Communication.Http.Broker.Department.Storage.Mock
{
    public class StorageCommunicatorProvider
    {
        private static IStorageCommunicator storageCommunicator;

        public static IStorageCommunicator GetStorageCommunicator(
            IDepartmentCommunicator departmentCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            if (storageCommunicator == null)
            {
                storageCommunicator = new StorageCommunicator(
                    departmentCommunicator,
                    serviceDiscoverer);
            }

            return storageCommunicator;
        }
    }
}
