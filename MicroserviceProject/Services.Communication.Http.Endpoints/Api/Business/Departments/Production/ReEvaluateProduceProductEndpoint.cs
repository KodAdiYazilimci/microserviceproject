using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Endpoints.Api.Business.Departments.Production
{
    public class ReEvaluateProduceProductEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "production.production.reevaluateproduceproduct";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
