using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateDefaultInventoryForNewWorkerCommandHandler
        : IRequestHandler<CreateDefaultInventoryForNewWorkerCommandRequest, CreateDefaultInventoryForNewWorkerCommandResponse>
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

        public async Task<CreateDefaultInventoryForNewWorkerCommandResponse> Handle(
            CreateDefaultInventoryForNewWorkerCommandRequest request,
            CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateDefaultInventoryForNewWorkerValidator.ValidateAsync(request.Inventory, cancellationTokenSource);

            return new CreateDefaultInventoryForNewWorkerCommandResponse()
            {
                Inventory =
                await
                _runtimeHandler.ExecuteResultMethod<Task<InventoryModel>>(
                    _inventoryService,
                    nameof(_inventoryService.CreateDefaultInventoryForNewWorkerAsync),
                    new object[] { request.Inventory, cancellationTokenSource })
            };
        }
    }
}
