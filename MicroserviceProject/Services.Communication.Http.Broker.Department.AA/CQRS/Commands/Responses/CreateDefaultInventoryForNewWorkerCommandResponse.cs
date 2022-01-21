using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses
{
    public class CreateDefaultInventoryForNewWorkerCommandResponse
    {
        public InventoryModel Inventory { get; set; }
    }
}
