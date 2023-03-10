using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;

namespace Services.Communication.Http.Broker.Department.Storage.Mock
{
    public class StorageCommunicatorProvider
    {
        private static StorageCommunicator storageCommunicator;

        public static StorageCommunicator GetStorageCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (storageCommunicator == null)
            {
                storageCommunicator = new StorageCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return storageCommunicator;
        }
    }
}
