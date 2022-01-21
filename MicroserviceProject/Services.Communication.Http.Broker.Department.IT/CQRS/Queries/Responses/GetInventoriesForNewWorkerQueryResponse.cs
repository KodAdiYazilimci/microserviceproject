using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses
{
    public class GetInventoriesForNewWorkerQueryResponse
    {
        public List<InventoryModel> Inventories { get; set; }
    }
}
