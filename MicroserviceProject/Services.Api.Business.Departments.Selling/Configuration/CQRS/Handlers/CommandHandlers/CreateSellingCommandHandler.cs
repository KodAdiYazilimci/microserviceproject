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
    public class CreateSellingCommandHandler : IRequestHandler<CreateSellingCommandRequest, CreateSellingCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly SellingService _sellingService;

        public CreateSellingCommandHandler(
            RuntimeHandler runtimeHandler,
            SellingService sellingService)
        {
            _runtimeHandler = runtimeHandler;
            _sellingService = sellingService;
        }

        public async Task<CreateSellingCommandResponse> Handle(CreateSellingCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateSellingValidator.ValidateAsync(request.Selling, cancellationTokenSource);

            return new CreateSellingCommandResponse()
            {
                CreatedSellingId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _sellingService,
                    nameof(_sellingService.CreateSellingAsync),
                    new object[] { request.Selling, cancellationTokenSource })
            };
        }
    }
}
