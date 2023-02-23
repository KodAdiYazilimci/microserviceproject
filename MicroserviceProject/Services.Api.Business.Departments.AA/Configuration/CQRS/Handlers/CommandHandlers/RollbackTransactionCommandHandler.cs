using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<AARollbackTransactionCommandRequest, AARollbackTransactionCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public RollbackTransactionCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<AARollbackTransactionCommandResponse> Handle(AARollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await RollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_inventoryService.ServiceName))
            {
                rollbackResult =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _inventoryService,
                        nameof(_inventoryService.RollbackTransactionAsync),
                        new object[] { request.Rollback, cancellationTokenSource });
            }

            return new AARollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
