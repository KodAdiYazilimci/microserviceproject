using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Selling.Abstract;

namespace Services.Communication.Http.Broker.Department.Selling.Mock
{
    public class SellingCommunicatorProvider
    {
        private static ISellingCommunicator sellingCommunicator;

        public static ISellingCommunicator GetSellingCommunicator(
            IRouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (sellingCommunicator == null)
            {
                sellingCommunicator = new SellingCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return sellingCommunicator;
        }
    }
}
