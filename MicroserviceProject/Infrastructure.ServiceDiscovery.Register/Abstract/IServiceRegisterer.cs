using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Register.Abstract
{
    public interface IServiceRegisterer
    {
        Task RegisterServiceAsync(ServiceModel service, CancellationTokenSource cancellationTokenSource);
    }
}
