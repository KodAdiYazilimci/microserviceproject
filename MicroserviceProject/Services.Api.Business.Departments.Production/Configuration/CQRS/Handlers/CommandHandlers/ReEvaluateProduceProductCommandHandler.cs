using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Production;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.CommandHandlers
{
    public class ReEvaluateProduceProductCommandHandler : IRequestHandler<ReEvaluateProduceProductCommandRequest, ReEvaluateProduceProductCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly ProductionService _productionService;

        public ReEvaluateProduceProductCommandHandler(
            RuntimeHandler runtimeHandler,
            ProductionService productionService)
        {
            _runtimeHandler = runtimeHandler;
            _productionService = productionService;
        }

        public async Task<ReEvaluateProduceProductCommandResponse> Handle(ReEvaluateProduceProductCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await ReEvaluateProduceProductValidator.ValidateAsync(request.ReferenceNumber, cancellationTokenSource);

            return new ReEvaluateProduceProductCommandResponse()
            {
                ExecutionResult =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _productionService,
                    nameof(_productionService.ReEvaluateProduceProductAsync),
                    new object[] { request.ReferenceNumber, cancellationTokenSource })
            };
        }
    }
}
