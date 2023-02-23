using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses
{
    public class ITGetInventoriesQueryResponse
    {
        public List<ITInventoryModel> Inventories { get; set; }
    }
}
