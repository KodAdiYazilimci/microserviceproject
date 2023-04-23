using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.Presentation.UI.Web.Identity
{
    public class LoginEndpoint : IEndpoint
    {
        public static string Path => "presentation.ui.web.identity.user.login";
        public string Url { get; set; } = "/Login";
        public string Name { get; set; } = Path;
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeaderModel> Headers { get; set; }
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>() { new HttpQueryModel() { Name = "redirectInfo", Required = true } };
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Anonymouse;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>()
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden
        };
    }
}
