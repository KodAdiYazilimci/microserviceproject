﻿using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Collections.Generic;
using System.Net;

namespace Services.Communication.Http.Broker.Authorization.Endpoints
{
    public class GetUserEndpoint : IEndpoint
    {
        public string Url { get; set; } = "/Auth/GetUser";
        public string Name { get; set; } = "authorization.auth.getuser";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>() { new HttpQueryModel() { Name = "token" } };
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Anonymouse;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized };
    }
}
