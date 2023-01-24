using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Providers;

using Services.Communication.Http.Broker.Authorization;

namespace Services.Communication.Http.Broker.Department.AA.Mock
{
    public class AACommunicatorProvider
    {
        private static AACommunicator aaCommunicator;

        public static AACommunicator GetAACommunicator(
            AuthorizationCommunicator authorizationCommunicator,
            InMemoryCacheDataProvider inMemoryCacheDataProvider,
            CredentialProvider credentialProvider,
            RouteProvider routeProvider,
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller)
        {
            if (aaCommunicator == null)
            {
                aaCommunicator = new AACommunicator(
                    authorizationCommunicator,
                    inMemoryCacheDataProvider,
                    credentialProvider,
                    routeProvider,
                    httpGetCaller,
                    httpPostCaller);
            }

            return aaCommunicator;
        }
    }
}
