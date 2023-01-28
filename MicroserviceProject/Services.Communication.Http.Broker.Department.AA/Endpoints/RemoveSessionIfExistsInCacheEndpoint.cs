using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Services.Communication.Http.Broker.Department.AA.Endpoints
{
    public class RemoveSessionIfExistsInCacheEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "aa.identity.removesessionifexistsincache";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
