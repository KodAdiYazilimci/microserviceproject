using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<RollbackTransactionCommandRequest, RollbackTransactionCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly RequestService _requestService;
        private readonly RollbackTransactionValidator _rollbackTransactionValidator;

        public RollbackTransactionCommandHandler(
            RuntimeHandler runtimeHandler,
            RequestService requestService,
            RollbackTransactionValidator rollbackTransactionValidator)
        {
            _runtimeHandler = runtimeHandler;
            _requestService = requestService;
            _rollbackTransactionValidator = rollbackTransactionValidator;
        }

        public async Task<RollbackTransactionCommandResponse> Handle(RollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _rollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_requestService.ServiceName))
            {
                rollbackResult =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _requestService,
                        nameof(_requestService.RollbackTransactionAsync),
                        new object[] { request.Rollback, cancellationTokenSource });
            }

            return new RollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
