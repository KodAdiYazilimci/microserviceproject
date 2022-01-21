using MediatR;

using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Buying.Models;

namespace Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests
{
    public class CreateInventoryRequestCommandRequest : IRequest<CreateInventoryRequestCommandResponse>
    {
        public InventoryRequestModel InventoryRequest { get; set; }
    }
}
