using Infrastructure.Routing.Providers.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;

namespace Services.Communication.Http.Broker.Authorization.Mock
{
    public class AuthorizationCommunicatorProvider
    {
        public static IAuthorizationCommunicator GetAuthorizationCommunicator(
            IRouteProvider routeProvider,
            ICommunicator communicator)
        {
            return new AuthorizationCommunicator(routeProvider, communicator);
        }
    }
}
