using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.IT.Mock
{
    public class ITCommunicatorProvider
    {
        private static ITCommunicator itCommunicator;

        public static ITCommunicator GetITCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider inMemoryCacheDataProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider)
        {
            if (itCommunicator == null)
            {
                itCommunicator = new ITCommunicator(
                    authorizationCommunicator,
                    inMemoryCacheDataProvider,
                    credentialProvider,
                    httpGetCaller,
                    httpPostCaller,
                    routeProvider);
            }

            return itCommunicator;
        }
    }
}
