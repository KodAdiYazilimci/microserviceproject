using Services.Communication.Http.Broker.Department.Selling.Models;

namespace Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Responses
{
    public class GetSoldsQueryResponse
    {
        public List<SellModel> Solds { get; set; }
    }
}
