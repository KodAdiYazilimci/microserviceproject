using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests
{
    public class CreateInventoryCommandRequest : IRequest<CreateInventoryCommandResponse>
    {
        public InventoryModel Inventory { get; set; }
    }
}
