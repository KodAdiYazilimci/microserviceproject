using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Infrastructure.ServiceDiscovery.Discoverer.Endpoints
{
    public class DiscoverEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
