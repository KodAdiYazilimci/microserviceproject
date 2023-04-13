namespace Infrastructure.ServiceDiscovery.Abstract
{
    public interface ISolidServiceConfiguration
    {
        public string Name { get; }
        public string RegisterAddress { get; }
        public string DiscoverAddress { get; }
    }
}
