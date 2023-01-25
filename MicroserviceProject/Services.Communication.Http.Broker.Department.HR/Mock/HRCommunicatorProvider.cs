using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.HR.Mock
{
    public class HRCommunicatorProvider
    {
        private static HRCommunicator hRCommunicator;

        public static HRCommunicator GetHRCommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider inMemoryCacheDataProvider,
            CredentialProvider credentialProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider
            )
        {
            if (hRCommunicator == null)
            {
                hRCommunicator = new HRCommunicator(
                    authorizationCommunicator,
                    inMemoryCacheDataProvider,
                    credentialProvider,
                    httpGetCaller,
                    httpPostCaller,
                    routeProvider);
            }

            return hRCommunicator;
        }
    }
}
