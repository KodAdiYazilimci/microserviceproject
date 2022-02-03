using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class AssignInventoryToWorkerCommandHandler : IRequestHandler<AssignInventoryToWorkerCommandRequest, AssignInventoryToWorkerCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public AssignInventoryToWorkerCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<AssignInventoryToWorkerCommandResponse> Handle(AssignInventoryToWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await AssignInventoryToWorkerValidator.ValidateAsync(request.Worker, cancellationTokenSource);

            return new AssignInventoryToWorkerCommandResponse()
            {
                Worker =
                await
                _runtimeHandler.ExecuteResultMethod<Task<WorkerModel>>(
                    _inventoryService,
                    nameof(_inventoryService.AssignInventoryToWorkerAsync),
                    new object[] { request.Worker, cancellationTokenSource })
            };
        }
    }
}
