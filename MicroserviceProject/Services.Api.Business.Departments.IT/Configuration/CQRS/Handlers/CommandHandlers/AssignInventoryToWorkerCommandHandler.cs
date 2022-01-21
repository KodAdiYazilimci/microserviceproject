using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class AssignInventoryToWorkerCommandHandler : IRequestHandler<AssignInventoryToWorkerCommandRequest, AssignInventoryToWorkerCommandResponse>
    {
        private readonly InventoryService _inventoryService;

        public AssignInventoryToWorkerCommandHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<AssignInventoryToWorkerCommandResponse> Handle(AssignInventoryToWorkerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await AssignInventoryToWorkerValidator.ValidateAsync(request.Worker, cancellationTokenSource);

            return new AssignInventoryToWorkerCommandResponse()
            {
                Worker = await _inventoryService.AssignInventoryToWorkerAsync(request.Worker, cancellationTokenSource)
            };
        }
    }
}
