using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Endpoints.Api.Business.Departments.Selling
{
    public class RemoveSessionIfExistsInCacheEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "selling.identity.removesessionifexistsincache";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeader> Headers { get; set; }
        public List<HttpQuery> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
