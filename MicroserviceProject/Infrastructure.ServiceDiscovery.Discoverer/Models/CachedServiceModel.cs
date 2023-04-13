namespace Infrastructure.ServiceDiscovery.Discoverer.Models
{
    public class CachedServiceModel : DiscoveredServiceModel
    {
        public DateTime ValidTo { get; set; }
    }
}
