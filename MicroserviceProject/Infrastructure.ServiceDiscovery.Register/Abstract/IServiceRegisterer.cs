using Infrastructure.ServiceDiscovery.Register.Models;

namespace Infrastructure.ServiceDiscovery.Register.Abstract
{
    public interface IServiceRegisterer
    {
        Task RegisterServiceAsync(RegisteredServiceModel service, CancellationTokenSource cancellationTokenSource);
    }
}
