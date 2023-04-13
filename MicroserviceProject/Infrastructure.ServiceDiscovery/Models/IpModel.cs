using System.Net.Sockets;

namespace Infrastructure.ServiceDiscovery.Models
{
    public class IpModel
    {
        public string Address { get; set; }
        public AddressFamily AddressFamily { get; set; }
    }
}
