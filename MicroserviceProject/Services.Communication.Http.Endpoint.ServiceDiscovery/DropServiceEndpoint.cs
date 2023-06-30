using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.ServiceDiscovery
{
    public class DropServiceEndpoint : IEndpoint
    {
        public static string Path => "servicediscoery.dropservice";
        public string Url { get; set; } = "/Registry/DropService";
        public string Name { get; set; } = Path;
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.DELETE;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>()
        {
            new HttpQueryModel(){ Name  = "serviceName", Required = true }
        };
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Anonymouse;
        public EndpointPurpose EndpointPurpose { get; set; } = EndpointPurpose.Operation;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>()
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest
        };
    }
}
