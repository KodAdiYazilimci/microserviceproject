using Services.Communication.Http.Broker.Department.Production.Models;

namespace Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Responses
{
    public class GetProductsQueryResponse
    {
        public List<ProductModel> Products { get; set; }
    }
}
