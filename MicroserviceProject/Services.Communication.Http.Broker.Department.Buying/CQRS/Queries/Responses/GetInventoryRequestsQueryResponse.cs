using Services.Communication.Http.Broker.Department.Buying.Models;

namespace Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Responses
{
    public class GetInventoryRequestsQueryResponse
    {
        public List<InventoryRequestModel> InventoryRequests { get; set; }
    }
}
