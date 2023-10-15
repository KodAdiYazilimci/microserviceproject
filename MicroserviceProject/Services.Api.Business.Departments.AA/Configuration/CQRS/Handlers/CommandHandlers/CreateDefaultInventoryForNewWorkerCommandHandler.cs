using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDefaultInventoryForNewWorkerCommandHandler :
        IRequestHandler<AACreateDefaultInventoryForNewWorkerCommandRequest, AACreateDefaultInventoryForNewWorkerCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;
        private readonly CreateDefaultInventoryForNewWorkerValidator _createDefaultInventoryForNewWorkerValidator;

        public CreateDefaultInventoryForNewWorkerCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService,
            CreateDefaultInventoryForNewWorkerValidator createDefaultInventoryForNewWorkerValidator)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
            _createDefaultInventoryForNewWorkerValidator = createDefaultInventoryForNewWorkerValidator;
        }

        public async Task<AACreateDefaultInventoryForNewWorkerCommandResponse> Handle(AACreateDefaultInventoryForNewWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createDefaultInventoryForNewWorkerValidator.ValidateAsync(request.DefaultInventoryForNewWorkerModel, cancellationTokenSource);

            await
            _runtimeHandler.ExecuteResultMethod<Task>(
                _inventoryService,
                nameof(_inventoryService.CreateDefaultInventoryForNewWorkerAsync),
                new object[] { request.DefaultInventoryForNewWorkerModel, cancellationTokenSource });

            return new AACreateDefaultInventoryForNewWorkerCommandResponse();
        }
    }
}
