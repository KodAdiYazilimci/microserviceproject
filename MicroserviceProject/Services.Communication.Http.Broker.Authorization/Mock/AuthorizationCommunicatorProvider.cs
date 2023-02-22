using Infrastructure.Communication.Http.Broker;
using Infrastructure.Routing.Providers;

namespace Services.Communication.Http.Broker.Authorization.Mock
{
    public class AuthorizationCommunicatorProvider
    {
        public static AuthorizationCommunicator GetAuthorizationCommunicator(
            HttpGetCaller httpGetCaller,
            HttpPostCaller httpPostCaller,
            RouteProvider routeProvider)
        {
            return new AuthorizationCommunicator(httpGetCaller, httpPostCaller, routeProvider);
        }
    }
}
