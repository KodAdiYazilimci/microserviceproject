namespace Infrastructure.ServiceDiscovery.Models
{
    public class ServiceModel
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public string DnsName { get; set; }
        public List<IpModel> IpAddresses { get; set; }

        public EndpointModel? GetEndpoint(string name)
        {
            if (Endpoints != null)
            {
                EndpointModel? endpoint = this.Endpoints.FirstOrDefault(x => x.Name == name);

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
