using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateCostCommandHandler : IRequestHandler<CreateCostCommandRequest, CreateCostCommandResponse>
    {
        private readonly CostService _costService;

        public CreateCostCommandHandler(CostService costService)
        {
            _costService = costService;
        }

        public async Task<CreateCostCommandResponse> Handle(CreateCostCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateCostValidator.ValidateAsync(request.Cost, cancellationTokenSource);

            return new CreateCostCommandResponse()
            {
                CreatedDecidecCostId = await _costService.CreateDecidedCostAsync(request.Cost, cancellationTokenSource)
            };
        }
    }
}
