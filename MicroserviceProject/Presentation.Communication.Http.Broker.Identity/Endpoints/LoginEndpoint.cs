using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Presentation.UI.Web.Endpoints
{
    public class LoginEndpoint : IEndpoint
    {
        public static string Path => "presentation.ui.web.identity.user.login";
        public string Url { get; set; } = "/User/Login";
        public string Name { get; set; } = Path;
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>();
        public AuthenticationType AuthenticationType { get; set; }
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>();
    }
}
