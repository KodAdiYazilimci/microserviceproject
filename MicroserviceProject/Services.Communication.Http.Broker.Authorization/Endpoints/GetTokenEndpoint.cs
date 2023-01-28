using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Authorization.Endpoints
{
    public class GetTokenEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "authorization.auth.gettoken";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
