using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.Department.Buying.Abstract;

namespace Services.Communication.Http.Broker.Department.Buying.Mock
{
    public class BuyingCommunicatorProvider
    {
        private static IBuyingCommunicator buyingCommunicator;

        public static IBuyingCommunicator GetBuyingCommunicator(
            RouteProvider routeProvider,
            IDepartmentCommunicator departmentCommunicator)
        {
            if (buyingCommunicator == null)
            {
                buyingCommunicator = new BuyingCommunicator(
                    routeProvider,
                    departmentCommunicator);
            }

            return buyingCommunicator;
        }
    }
}
