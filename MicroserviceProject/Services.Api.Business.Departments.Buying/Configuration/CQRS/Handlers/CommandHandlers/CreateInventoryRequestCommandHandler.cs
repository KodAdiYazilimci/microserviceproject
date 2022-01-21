using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateInventoryRequestCommandHandler : IRequestHandler<CreateInventoryRequestCommandRequest, CreateInventoryRequestCommandResponse>
    {
        private readonly RequestService _requestService;

        public CreateInventoryRequestCommandHandler(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<CreateInventoryRequestCommandResponse> Handle(CreateInventoryRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

            return new CreateInventoryRequestCommandResponse()
            {
                CreatedInventoryRequestId = await _requestService.CreateInventoryRequestAsync(request.InventoryRequest, cancellationTokenSource)
            };
        }
    }
}
