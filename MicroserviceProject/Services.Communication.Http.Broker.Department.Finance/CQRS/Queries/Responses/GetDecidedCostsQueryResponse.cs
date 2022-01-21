using Services.Communication.Http.Broker.Department.Finance.Models;

namespace Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses
{
    public class GetDecidedCostsQueryResponse
    {
        public List<DecidedCostModel> DecidedCosts { get; set; }
    }
}
