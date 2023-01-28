using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Authorization.Endpoints
{
    public class GetUserEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "authorization.auth.getuser";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
