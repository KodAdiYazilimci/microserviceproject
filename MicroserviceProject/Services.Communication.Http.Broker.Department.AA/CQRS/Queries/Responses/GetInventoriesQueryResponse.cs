using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses
{
    public class GetInventoriesQueryResponse
    {
        public List<InventoryModel> Inventories { get; set; }
    }
}
