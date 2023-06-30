using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.Department.AA
{
    public class AACreateDefaultInventoryForNewWorkerEndpoint : IEndpoint
    {
        public static string Path => "aa.inventory.createdefaultinventoryfornewworker";
        public string Url { get; set; } = "/Inventory/CreateDefaultInventoryForNewWorker";
        public string Name { get; set; } = Path;
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.PUT;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>();
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Token;
        public EndpointPurpose EndpointPurpose { get; set; } = EndpointPurpose.Operation;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized };
    }
}
