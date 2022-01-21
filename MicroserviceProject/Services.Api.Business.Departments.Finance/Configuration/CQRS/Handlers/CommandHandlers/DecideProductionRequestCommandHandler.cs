using MediatR;

using Services.Business.Departments.Finance.Services;
using Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Finance.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DecideProductionRequestCommandHandler : IRequestHandler<DecideProductionRequestCommandRequest, DecideProductionRequestCommandResponse>
    {
        private readonly ProductionRequestService _productionRequestService;

        public DecideProductionRequestCommandHandler(ProductionRequestService productionRequestService)
        {
            _productionRequestService = productionRequestService;
        }

        public async Task<DecideProductionRequestCommandResponse> Handle(DecideProductionRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await DecideProductionRequestValidator.ValidateAsync(request.ProductionRequest, cancellationTokenSource);

            if (request.ProductionRequest.Approved)
                return new DecideProductionRequestCommandResponse()
                {
                    ProductionRequestId = await _productionRequestService.ApproveProductionRequestAsync(request.ProductionRequest.Id, cancellationTokenSource)
                };
            else
                return new DecideProductionRequestCommandResponse()
                {
                    ProductionRequestId = await _productionRequestService.RejectProductionRequestAsync(request.ProductionRequest.Id, cancellationTokenSource)
                };
        }
    }
}
