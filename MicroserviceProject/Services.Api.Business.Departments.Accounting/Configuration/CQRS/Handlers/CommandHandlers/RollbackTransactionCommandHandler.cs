using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<AccountingRollbackTransactionCommandRequest, AccountingRollbackTransactionCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;
        private readonly RollbackTransactionValidator _rollbackTransactionValidator;

        public RollbackTransactionCommandHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService,
            RollbackTransactionValidator rollbackTransactionValidator)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
            _rollbackTransactionValidator = rollbackTransactionValidator;
        }

        public async Task<AccountingRollbackTransactionCommandResponse> Handle(AccountingRollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _rollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_bankService.ServiceName))
            {
                rollbackResult =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _bankService,
                        nameof(_bankService.RollbackTransactionAsync),
                        new object[] { request.Rollback, cancellationTokenSource });
            }

            return new AccountingRollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
