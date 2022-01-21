using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDefaultInventoryForNewWorkerCommandHandler : IRequestHandler<CreateDefaultInventoryForNewWorkerCommandRequest, CreateDefaultInventoryForNewWorkerCommandResponse>
    {
        private readonly InventoryService _inventoryService;

        public CreateDefaultInventoryForNewWorkerCommandHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<CreateDefaultInventoryForNewWorkerCommandResponse> Handle(CreateDefaultInventoryForNewWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateDefaultInventoryForNewWorkerValidator.ValidateAsync(request.Inventory, cancellationTokenSource);

            return new CreateDefaultInventoryForNewWorkerCommandResponse()
            {
                Inventory = await _inventoryService.CreateDefaultInventoryForNewWorkerAsync(request.Inventory, cancellationTokenSource)
            };
        }
    }
}
