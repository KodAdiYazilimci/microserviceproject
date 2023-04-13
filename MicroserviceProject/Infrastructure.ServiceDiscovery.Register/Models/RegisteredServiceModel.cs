using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Register.Models
{
    public class RegisteredServiceModel
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public List<IEndpoint> Endpoints { get; set; }
        public string DnsName { get; set; }
        public List<IpModel> IpAddresses { get; set; }

        public IEndpoint GetEndpoint(string name)
        {
            if (Endpoints != null)
            {
                IEndpoint? endpoint = Endpoints.FirstOrDefault(x => x.Name == name);

                if (endpoint != null)
                {
                    endpoint.Url = $"{Protocol}://{DnsName}:{Port}{endpoint.Url}";

                    return endpoint;
                }
            }

            return null;
        }
    }
}
