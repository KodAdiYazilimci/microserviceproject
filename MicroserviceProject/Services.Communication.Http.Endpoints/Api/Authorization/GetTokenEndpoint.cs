using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Endpoints.Api.Authorization
{
    public class GetTokenEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "authorization.auth.gettoken";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeader> Headers { get; set; }
        public List<HttpQuery> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
