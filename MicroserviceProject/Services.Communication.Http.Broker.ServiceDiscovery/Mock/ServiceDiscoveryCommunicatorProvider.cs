using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Services.Communication.Http.Broker.Abstract;
using Services.Communication.Http.Broker.ServiceDiscovery.Abstract;

namespace Services.Communication.Http.Broker.ServiceDiscovery.Mock
{
    public class ServiceDiscoveryCommunicatorProvider
    {
        public static IServiceDiscoveryCommunicator GetServiceDiscoveryCommunicator(
            ICommunicator communicator,
            IServiceDiscoverer serviceDiscoverer)
        {
            return new ServiceDiscoveryCommunicator(communicator, serviceDiscoverer);
        }
    }
}
