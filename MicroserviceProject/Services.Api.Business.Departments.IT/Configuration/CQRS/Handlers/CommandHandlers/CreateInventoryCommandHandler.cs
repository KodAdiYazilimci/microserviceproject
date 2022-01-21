using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommandRequest, CreateInventoryCommandResponse>
    {
        private readonly InventoryService _inventoryService;

        public CreateInventoryCommandHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<CreateInventoryCommandResponse> Handle(CreateInventoryCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateInventoryValidator.ValidateAsync(request.Inventory, cancellationTokenSource);

            return new CreateInventoryCommandResponse()
            {
                CreatedInventoryId = await _inventoryService.CreateInventoryAsync(request.Inventory, cancellationTokenSource)
            };
        }
    }
}
