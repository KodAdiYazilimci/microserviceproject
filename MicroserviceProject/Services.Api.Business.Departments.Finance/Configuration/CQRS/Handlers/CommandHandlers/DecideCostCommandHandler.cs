using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DecideCostCommandHandler : IRequestHandler<DecideCostCommandRequest, DecideCostCommandResponse>
    {
        private readonly CostService _costService;

        public DecideCostCommandHandler(CostService costService)
        {
            _costService = costService;
        }

        public async Task<DecideCostCommandResponse> Handle(DecideCostCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await DecideCostValidator.ValidateAsync(request.Cost, cancellationTokenSource);

            if (request.Cost.Approved)
                return new DecideCostCommandResponse()
                {
                    Result = await _costService.ApproveCostAsync(request.Cost.Id, cancellationTokenSource)
                };
            else
                return new DecideCostCommandResponse()
                {
                    Result = await _costService.RejectCostAsync(request.Cost.Id, cancellationTokenSource)
                };
        }
    }
}
