using Infrastructure.ServiceDiscovery.Models;

namespace Services.Api.ServiceDiscovery.Dto
{
    public class ServiceDto
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public new List<EndpointDto> Endpoints { get; set; }
        public string DnsName { get; set; }
        public List<IpModel> IpAddresses { get; set; }
    }
}
