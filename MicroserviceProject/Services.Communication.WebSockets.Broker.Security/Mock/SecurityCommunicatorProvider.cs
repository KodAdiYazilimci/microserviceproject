using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.WebSockets.Broker.Abstract;
using Services.Communication.WebSockets.Broker.Security.Abstract;

namespace Services.Communication.WebSockets.Broker.Security.Mock
{
    public class SecurityCommunicatorProvider
    {
        public static ISecurityCommunicator GetSecurityCommunicator(
            ISocketCommunicator socketCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            return new SecurityCommunicator(socketCommunicator, serviceDiscoverer);
        }
    }
}
