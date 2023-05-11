using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

namespace Services.Communication.Http.Broker.ServiceDiscovery.Abstract
{
    public interface IServiceDiscoveryCommunicator
    {
        Task<ServiceResultModel> DropServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource);
        Task<ServiceResultModel<List<DiscoveredServiceModel>>> GetDiscoveredServicesAsync(CancellationTokenSource cancellationTokenSource);
    }
}
