using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses
{
    public class ITGetInventoriesForNewWorkerQueryResponse
    {
        public List<ITDefaultInventoryForNewWorkerModel> Inventories { get; set; }
    }
}
