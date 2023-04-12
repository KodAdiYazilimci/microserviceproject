using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Infrastructure.ServiceDiscovery.Register.Endpoints
{
    public class RegisterEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Queries { get; set; } = new Dictionary<string, string>();
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
