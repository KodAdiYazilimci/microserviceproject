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

        public RollbackTransactionCommandHandler(
            RuntimeHandler runtimeHandler,
            RequestService requestService)
        {
            _runtimeHandler = runtimeHandler;
            _requestService = requestService;
        }

        public async Task<RollbackTransactionCommandResponse> Handle(RollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await RollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

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
