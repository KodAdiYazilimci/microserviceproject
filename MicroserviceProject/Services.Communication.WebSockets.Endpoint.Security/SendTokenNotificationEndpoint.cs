﻿using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.WebSockets.Endpoint.Security
{
    public class SendTokenNotificationEndpoint : IEndpoint
    {
        public static string Path => "services.websockets.security.sendtokennotification";
        public string Url { get; set; } = "/SendTokenNotification";
        public string Name { get; set; } = Path;
        public object? Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeaderModel> Headers { get; set; }
        public List<HttpQueryModel> Queries { get; set; }
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Token;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>()
        {
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized
        };
    }
}
