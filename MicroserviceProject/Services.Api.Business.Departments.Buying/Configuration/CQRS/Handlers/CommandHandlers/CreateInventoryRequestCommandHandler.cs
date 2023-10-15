using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateInventoryRequestCommandHandler : IRequestHandler<CreateInventoryRequestCommandRequest, CreateInventoryRequestCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly RequestService _requestService;
        private readonly CreateInventoryRequestValidator _createInventoryRequestValidator;

        public CreateInventoryRequestCommandHandler(
            RuntimeHandler runtimeHandler,
            RequestService requestService,
            CreateInventoryRequestValidator createInventoryRequestValidator)
        {
            _runtimeHandler = runtimeHandler;
            _requestService = requestService;
            _createInventoryRequestValidator = createInventoryRequestValidator;
        }

        public async Task<CreateInventoryRequestCommandResponse> Handle(CreateInventoryRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

            return new CreateInventoryRequestCommandResponse()
            {
                CreatedInventoryRequestId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _requestService,
                    nameof(_requestService.CreateInventoryRequestAsync),
                    new object[] { request.InventoryRequest, cancellationTokenSource })
            };
        }
    }
}
