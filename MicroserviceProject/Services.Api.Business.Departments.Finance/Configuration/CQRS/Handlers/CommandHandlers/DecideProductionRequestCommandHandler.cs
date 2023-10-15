using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DecideProductionRequestCommandHandler : IRequestHandler<DecideProductionRequestCommandRequest, DecideProductionRequestCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly ProductionRequestService _productionRequestService;
        private readonly DecideProductionRequestValidator _decideProductionRequestValidator;

        public DecideProductionRequestCommandHandler(
            RuntimeHandler runtimeHandler,
            ProductionRequestService productionRequestService,
            DecideProductionRequestValidator decideProductionRequestValidator)
        {
            _runtimeHandler = runtimeHandler;
            _productionRequestService = productionRequestService;
            _decideProductionRequestValidator = decideProductionRequestValidator;
        }

        public async Task<DecideProductionRequestCommandResponse> Handle(DecideProductionRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _decideProductionRequestValidator.ValidateAsync(request.ProductionRequest, cancellationTokenSource);

            if (request.ProductionRequest.Approved)
            {
                return new DecideProductionRequestCommandResponse()
                {
                    ProductionRequestId =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _productionRequestService,
                        nameof(_productionRequestService.ApproveProductionRequestAsync),
                        new object[] { request.ProductionRequest.Id, cancellationTokenSource })
                };
            }
            else
            {
                return new DecideProductionRequestCommandResponse()
                {
                    ProductionRequestId =
                    await
                    _runtimeHandler.ExecuteResultMethod<Task<int>>(
                        _productionRequestService,
                        nameof(_productionRequestService.RejectProductionRequestAsync),
                        new object[] { request.ProductionRequest.Id, cancellationTokenSource })
                };
            }
        }
    }
}
