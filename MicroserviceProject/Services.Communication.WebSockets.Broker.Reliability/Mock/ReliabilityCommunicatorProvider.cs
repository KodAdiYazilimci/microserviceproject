using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.WebSockets.Broker.Abstract;
using Services.Communication.WebSockets.Broker.Reliability.Abstract;

namespace Services.Communication.WebSockets.Broker.Reliability.Mock
{
    public class ReliabilityCommunicatorProvider
    {
        public static IReliabilityCommunicator GetReliabilityCommunicator(
            ISocketCommunicator socketCommunicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            return new ReliabilityCommunicator(socketCommunicator, serviceDiscoverer);
        }
    }
}
