using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

using System.Collections.Generic;

namespace Presentation.UI.Web.Endpoints
{
    public class LoginEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "presentation.ui.web.identity.user.login";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
