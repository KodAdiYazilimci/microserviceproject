using Infrastructure.ServiceDiscovery.Discoverer.Models;

namespace Infrastructure.ServiceDiscovery.Discoverer.Abstract
{
    public interface IServiceDiscoverer
    {
        Task<CachedServiceModel> GetServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource);
    }
}
