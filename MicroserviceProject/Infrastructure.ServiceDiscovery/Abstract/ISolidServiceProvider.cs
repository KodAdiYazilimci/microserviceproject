using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Abstract
{
    public interface ISolidServiceProvider
    {
        SolidServiceModel GetSolidService();
    }
}
