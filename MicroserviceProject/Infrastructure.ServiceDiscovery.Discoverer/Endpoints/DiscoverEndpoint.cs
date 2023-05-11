using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Infrastructure.ServiceDiscovery.Discoverer.Endpoints
{
    public class DiscoverEndpoint : IAuthenticatedEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; }
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>();
        public IEndpointAuthentication EndpointAuthentication { get; set; } = new AnonymouseAuthentication();
        public AuthenticationType AuthenticationType { get; set; }
        public EndpointPurpose EndpointPurpose { get; set; } = EndpointPurpose.Operation;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>();
    }
}
