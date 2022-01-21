using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Production;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.CommandHandlers
{
    public class ReEvaluateProduceProductCommandHandler : IRequestHandler<ReEvaluateProduceProductCommandRequest, ReEvaluateProduceProductCommandResponse>
    {
        private readonly ProductionService _productionService;

        public ReEvaluateProduceProductCommandHandler(ProductionService productionService)
        {
            _productionService = productionService;
        }

        public async Task<ReEvaluateProduceProductCommandResponse> Handle(ReEvaluateProduceProductCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await ReEvaluateProduceProductValidator.ValidateAsync(request.ReferenceNumber, cancellationTokenSource);

            return new ReEvaluateProduceProductCommandResponse()
            {
                ExecutionResult = await _productionService.ReEvaluateProduceProductAsync(request.ReferenceNumber, cancellationTokenSource)
            };
        }
    }
}
