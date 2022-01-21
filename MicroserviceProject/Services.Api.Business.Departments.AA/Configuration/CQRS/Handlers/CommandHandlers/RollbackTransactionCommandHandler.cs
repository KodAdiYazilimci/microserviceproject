using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<RollbackTransactionCommandRequest, RollbackTransactionCommandResponse>
    {
        private readonly InventoryService _inventoryService;

        public RollbackTransactionCommandHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<RollbackTransactionCommandResponse> Handle(RollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await RollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_inventoryService.ServiceName))
            {
                rollbackResult = await _inventoryService.RollbackTransactionAsync(request.Rollback, cancellationTokenSource);
            }

            return new RollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
