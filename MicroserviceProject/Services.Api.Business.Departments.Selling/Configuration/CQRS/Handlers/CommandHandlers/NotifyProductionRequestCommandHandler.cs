using MediatR;

using Services.Api.Business.Departments.Selling.Services;
using Services.Api.Business.Departments.Selling.Util.Validation.Selling;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Configuration.CQRS.Handlers.CommandHandlers
{
    public class NotifyProductionRequestCommandHandler : IRequestHandler<NotifyProductionRequestCommandRequest, NotifyProductionRequestCommandResponse>
    {
        private readonly SellingService _sellingService;

        public NotifyProductionRequestCommandHandler(SellingService sellingService)
        {
            _sellingService = sellingService;
        }

        public async Task<NotifyProductionRequestCommandResponse> Handle(NotifyProductionRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await NotifyProductionRequestValidator.ValidateAsync(request.ProductionRequest, cancellationTokenSource);

            return new NotifyProductionRequestCommandResponse()
            {
                SellingId = await _sellingService.NotifyProductionRequestAsync(request.ProductionRequest, cancellationTokenSource)
            };
        }
    }
}
