using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.Department.CR
{
    public class HealthCheckEndpoint : IEndpoint
    {
        public static string Path => "Services.Api.Business.Departments.CR.Health";
        public string Url { get; set; } = "/health";
        public string Name { get; set; } = Path;
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeaderModel> Headers { get; set; }
        public List<HttpQueryModel> Queries { get; set; }
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Anonymouse;
        public EndpointPurpose EndpointPurpose { get; set; } = EndpointPurpose.HealthCheck;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>()
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest
        };
    }
}
