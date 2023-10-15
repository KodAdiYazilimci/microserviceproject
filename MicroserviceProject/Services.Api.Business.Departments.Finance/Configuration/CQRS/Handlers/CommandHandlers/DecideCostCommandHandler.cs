using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DecideCostCommandHandler : IRequestHandler<DecideCostCommandRequest, DecideCostCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly CostService _costService;
        private readonly DecideCostValidator _decideCostValidator;

        public DecideCostCommandHandler(
            RuntimeHandler runtimeHandler,
            CostService costService,
            DecideCostValidator decideCostValidator)
        {
            _runtimeHandler = runtimeHandler;
            _costService = costService;
            _decideCostValidator = decideCostValidator;
        }

        public async Task<DecideCostCommandResponse> Handle(DecideCostCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _decideCostValidator.ValidateAsync(request.Cost, cancellationTokenSource);

            if (request.Cost.Approved)
            {
                return new DecideCostCommandResponse()
                {
                    Result =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _costService,
                        nameof(_costService.ApproveCostAsync),
                        new object[] { request.Cost.Id, cancellationTokenSource })
                };
            }
            else
            {
                return new DecideCostCommandResponse()
                {
                    Result =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _costService,
                        nameof(_costService.RejectCostAsync),
                        new object[] { request.Cost.Id, cancellationTokenSource })
                };
            }
        }
    }
}
