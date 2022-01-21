using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<RollbackTransactionCommandRequest, RollbackTransactionCommandResponse>
    {
        private readonly BankService _bankService;

        public RollbackTransactionCommandHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<RollbackTransactionCommandResponse> Handle(RollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await RollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_bankService.ServiceName))
            {
                rollbackResult = await _bankService.RollbackTransactionAsync(request.Rollback, cancellationTokenSource);
            }

            return new RollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
