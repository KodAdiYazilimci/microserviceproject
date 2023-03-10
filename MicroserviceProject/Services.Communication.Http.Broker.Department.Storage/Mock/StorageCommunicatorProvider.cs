using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Storage.Abstract;

namespace Services.Communication.Http.Broker.Department.Storage.Mock
{
    public class StorageCommunicatorProvider
    {
        private static IStorageCommunicator storageCommunicator;

        public static IStorageCommunicator GetStorageCommunicator(
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
