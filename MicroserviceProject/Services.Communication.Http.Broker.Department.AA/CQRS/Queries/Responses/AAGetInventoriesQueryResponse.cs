using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses
{
    public class AAGetInventoriesQueryResponse
    {
        public List<AAInventoryModel> Inventories { get; set; }
    }
}
