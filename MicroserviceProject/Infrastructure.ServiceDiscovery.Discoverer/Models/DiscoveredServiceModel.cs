using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.ServiceDiscovery.Models;

namespace Infrastructure.ServiceDiscovery.Discoverer.Models
{
    public class DiscoveredServiceModel
    {
        public string ServiceName { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public string DnsName { get; set; }
        public List<IpModel> IpAddresses { get; set; }

        public IEndpoint GetEndpoint(string name)
        {
            if (Endpoints != null)
            {
                IEndpoint? endpoint = Endpoints.FirstOrDefault(x => x.Name == name);

                if (endpoint != null)
                {
                    EndpointModel shadowEndpoint = new EndpointModel();
                    shadowEndpoint.Name = endpoint.Name;
                    shadowEndpoint.AuthenticationType = endpoint.AuthenticationType;
                    shadowEndpoint.EndpointAuthentication = endpoint.EndpointAuthentication;
                    shadowEndpoint.HttpAction = endpoint.HttpAction;
                    shadowEndpoint.Headers = endpoint.Headers;
                    shadowEndpoint.Payload = endpoint.Payload;
                    shadowEndpoint.Queries = endpoint.Queries;
                    shadowEndpoint.StatusCodes = endpoint.StatusCodes;

                    shadowEndpoint.Url = $"{Protocol}://{DnsName}:{Port}{endpoint.Url}";

                    return shadowEndpoint;
                }
            }

            return null;
        }
    }
}
