using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.Authorization.Abstract;

namespace Services.Communication.Http.Broker.Authorization.Mock
{
    public class AuthorizationCommunicatorProvider
    {
        public static IAuthorizationCommunicator GetAuthorizationCommunicator(
            ICommunicator communicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            return new AuthorizationCommunicator(communicator, serviceDiscoverer);
        }
    }
}
