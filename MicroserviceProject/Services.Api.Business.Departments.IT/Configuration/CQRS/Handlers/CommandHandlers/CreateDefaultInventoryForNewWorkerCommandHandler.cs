using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDefaultInventoryForNewWorkerCommandHandler
        : IRequestHandler<ITCreateDefaultInventoryForNewWorkerCommandRequest, ITCreateDefaultInventoryForNewWorkerCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public CreateDefaultInventoryForNewWorkerCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<ITCreateDefaultInventoryForNewWorkerCommandResponse> Handle(
            ITCreateDefaultInventoryForNewWorkerCommandRequest request,
            CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateDefaultInventoryForNewWorkerValidator.ValidateAsync(request.DefaultInventoryForNewWorkerModel, cancellationTokenSource);

            await
            _runtimeHandler.ExecuteResultMethod<Task>(
                _inventoryService,
                nameof(_inventoryService.CreateDefaultInventoryForNewWorkerAsync),
                new object[] { request.DefaultInventoryForNewWorkerModel, cancellationTokenSource });

            return new ITCreateDefaultInventoryForNewWorkerCommandResponse();
        }
    }
}
