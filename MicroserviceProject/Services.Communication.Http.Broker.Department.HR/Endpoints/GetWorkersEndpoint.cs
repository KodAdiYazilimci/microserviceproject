using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Broker.Department.HR.Endpoints
{
    public class GetWorkersEndpoint : IEndpoint
    {
        public string Url { get; set; } = "/Person/GetWorkers";
        public string Name { get; set; } = "hr.person.getworkers";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>();
        public IEndpointAuthentication EndpointAuthentication { get; set; }
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Token;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized };
    }
}
