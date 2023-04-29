using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.WebSockets.Endpoint.Reliability
{
    public class SendErrorNotificationEndpoint : IEndpoint
    {
        public static string Path => "services.webcockets.reliability.senderrornotification";
        public string Url { get; set; } = "/SendErrorNotification";
        public string Name { get; set; } = Path;
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeaderModel> Headers { get; set; }
        public List<HttpQueryModel> Queries { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public EndpointPurpose EndpointPurpose { get; set; } = EndpointPurpose.Operation;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>()
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden,
        };
    }
}
