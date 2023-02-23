using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateInventory;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateInventoryCommandHandler : IRequestHandler<ITCreateInventoryCommandRequest, ITCreateInventoryCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public CreateInventoryCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<ITCreateInventoryCommandResponse> Handle(ITCreateInventoryCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateInventoryValidator.ValidateAsync(request.Inventory, cancellationTokenSource);

            return new ITCreateInventoryCommandResponse()
            {
                CreatedInventoryId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _inventoryService,
                    nameof(_inventoryService.CreateInventoryAsync),
                    new object[] { request.Inventory, cancellationTokenSource })
            };
        }
    }
}
