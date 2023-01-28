using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Gateway.Endpoints
{
    public class GetDepartmentsEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "gateway.public.hr.getdepartments";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
