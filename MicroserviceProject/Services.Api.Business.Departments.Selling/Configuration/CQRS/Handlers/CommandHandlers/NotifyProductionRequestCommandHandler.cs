using MediatR;

using Services.Api.Business.Departments.Selling.Services;
using Services.Api.Business.Departments.Selling.Util.Validation.Selling;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Configuration.CQRS.Handlers.CommandHandlers
{
    public class NotifyProductionRequestCommandHandler : IRequestHandler<NotifyProductionRequestCommandRequest, NotifyProductionRequestCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly SellingService _sellingService;

        public NotifyProductionRequestCommandHandler(
            RuntimeHandler runtimeHandler,
            SellingService sellingService)
        {
            _runtimeHandler = runtimeHandler;
            _sellingService = sellingService;
        }

        public async Task<NotifyProductionRequestCommandResponse> Handle(NotifyProductionRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await NotifyProductionRequestValidator.ValidateAsync(request.ProductionRequest, cancellationTokenSource);

            return new NotifyProductionRequestCommandResponse()
            {
                SellingId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _sellingService,
                    nameof(_sellingService.NotifyProductionRequestAsync),
                    new object[] { request.ProductionRequest, cancellationTokenSource })
            };
        }
    }
}
