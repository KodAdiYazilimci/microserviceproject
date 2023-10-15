using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class InformInventoryRequestCommandHandler : IRequestHandler<ITInformInventoryRequestCommandRequest, ITInformInventoryRequestCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;
        private readonly InformInventoryRequestValidator _informInventoryRequestValidator;

        public InformInventoryRequestCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService,
            InformInventoryRequestValidator informInventoryRequestValidator)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
            _informInventoryRequestValidator = informInventoryRequestValidator;
        }

        public async Task<ITInformInventoryRequestCommandResponse> Handle(ITInformInventoryRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _informInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

            await _runtimeHandler.ExecuteResultMethod<Task>(
                _inventoryService,
                nameof(_inventoryService.InformInventoryRequestAsync),
                new object[] { request.InventoryRequest, cancellationTokenSource });

            return new ITInformInventoryRequestCommandResponse();
        }
    }
}
