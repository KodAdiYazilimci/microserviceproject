using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.AssignInventoryToWorker;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
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
