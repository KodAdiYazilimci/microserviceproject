using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.Department.Production
{
    public class ReEvaluateProduceProductEndpoint : IEndpoint
    {
        public string Url { get; set; } = "/Production/ReEvaluateProduceProduct";
        public string Name { get; set; } = "production.production.reevaluateproduceproduct";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.POST;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>()
        {
            new HttpQueryModel(){ Name = "referenceNumber", Required = true }
        };
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Token;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized };
    }
}
