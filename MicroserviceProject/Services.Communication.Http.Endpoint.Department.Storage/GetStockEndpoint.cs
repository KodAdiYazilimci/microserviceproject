using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Services.Communication.Http.Endpoint.Department.Storage
{
    public class GetStockEndpoint : IEndpoint
    {
        public string Url { get; set; } = "/Stock/GetStocks";
        public string Name { get; set; } = "storage.stock.getstock";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeaderModel> Headers { get; set; } = new List<HttpHeaderModel>();
        public List<HttpQueryModel> Queries { get; set; } = new List<HttpQueryModel>()
        {
            new HttpQueryModel() { Name = "productId", Required = true }
        };
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Token;
        public List<HttpStatusCode> StatusCodes { get; set; } = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized };
    }
}
