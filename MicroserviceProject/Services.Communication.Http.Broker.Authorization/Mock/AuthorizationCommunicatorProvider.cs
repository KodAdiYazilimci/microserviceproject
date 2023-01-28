using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Authorization.Mock
{
    public class AuthorizationCommunicatorProvider
    {
        private static AuthorizationCommunicator authorizationCommunicator;

        public static AuthorizationCommunicator GetAuthorizationCommunicator(
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider)
        {
            if (authorizationCommunicator == null)
            {
                authorizationCommunicator = new AuthorizationCommunicator(
                    httpGetCaller,
                    httpPostCaller,
                    routeProvider);
            }

            return authorizationCommunicator;
        }
    }
}
