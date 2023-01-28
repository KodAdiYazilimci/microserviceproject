using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.Selling.Mock
{
    public class SellingCommunicatorProvider
    {
        private static SellingCommunicator sellingCommunicator;

        public static SellingCommunicator GetSellingCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider inMemoryCacheDataProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider)
        {
            if (sellingCommunicator == null)
            {
                sellingCommunicator = new SellingCommunicator(
                    authorizationCommunicator,
                    inMemoryCacheDataProvider,
                    credentialProvider,
                    httpGetCaller,
                    httpPostCaller,
                    routeProvider);
            }

            return sellingCommunicator;
        }
    }
}
