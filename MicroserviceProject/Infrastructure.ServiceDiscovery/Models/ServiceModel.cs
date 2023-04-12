namespace Infrastructure.ServiceDiscovery.Models
{
    public class ServiceModel
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
    }
}
