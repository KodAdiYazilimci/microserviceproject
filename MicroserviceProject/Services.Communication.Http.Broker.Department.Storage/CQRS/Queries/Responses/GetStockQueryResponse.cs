using Services.Communication.Http.Broker.Department.Storage.Models;

namespace Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Responses
{
    public class GetStockQueryResponse
    {
        public StockModel Stock { get; set; }
    }
}
