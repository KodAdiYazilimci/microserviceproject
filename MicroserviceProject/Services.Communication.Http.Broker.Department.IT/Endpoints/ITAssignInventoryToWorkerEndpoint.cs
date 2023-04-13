﻿using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Broker.Department.IT.Endpoints
{
    public class ITAssignInventoryToWorkerEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "it.inventory.assigninventorytoworker";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>();
        public IEndpointAuthentication EndpointAuthentication { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>();
    }
}
