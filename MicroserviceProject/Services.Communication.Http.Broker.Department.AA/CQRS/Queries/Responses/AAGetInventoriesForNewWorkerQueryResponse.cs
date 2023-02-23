using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses
{
    public class AAGetInventoriesForNewWorkerQueryResponse
    {
        public List<AADefaultInventoryForNewWorkerModel> Inventories { get; set; }
    }
}
