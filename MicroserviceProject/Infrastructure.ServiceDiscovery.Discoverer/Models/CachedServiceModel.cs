using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Discoverer.Models
{
    public class CachedServiceModel : ServiceModel
    {
        public DateTime ValidTo { get; set; }
    }
}
