using System.Net.Sockets;

namespace Services.Api.ServiceDiscovery.Dto
{
    public class IpDto
    {
        public string Address { get; set; }
        public AddressFamily AddressFamily { get; set; }
    }
}
