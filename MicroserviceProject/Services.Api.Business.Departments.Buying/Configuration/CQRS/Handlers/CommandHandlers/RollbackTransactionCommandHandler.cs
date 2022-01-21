using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Transaction;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class RollbackTransactionCommandHandler : IRequestHandler<RollbackTransactionCommandRequest, RollbackTransactionCommandResponse>
    {
        private readonly RequestService _requestService;

        public RollbackTransactionCommandHandler(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<RollbackTransactionCommandResponse> Handle(RollbackTransactionCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await RollbackTransactionValidator.ValidateAsync(request.Rollback, cancellationTokenSource);

            int rollbackResult = 0;

            if (request.Rollback.Modules.Contains(_requestService.ServiceName))
            {
                rollbackResult = await _requestService.RollbackTransactionAsync(request.Rollback, cancellationTokenSource);
            }

            return new RollbackTransactionCommandResponse() { Result = rollbackResult };
        }
    }
}
