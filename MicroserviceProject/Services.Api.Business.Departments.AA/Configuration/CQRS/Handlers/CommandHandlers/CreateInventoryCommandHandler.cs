using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateInventory;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateInventoryCommandHandler : IRequestHandler<AACreateInventoryCommandRequest, AACreateInventoryCommandResponse>
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

        public async Task<AACreateInventoryCommandResponse> Handle(AACreateInventoryCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateInventoryValidator.ValidateAsync(request.Inventory, cancellationTokenSource);

            return new AACreateInventoryCommandResponse()
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
