using MediatR;

using Services.Api.Business.Departments.AA.Services;
using Services.Api.Business.Departments.AA.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.AA.Configuration.CQRS.Handlers.CommandHandlers
{
    public class InformInventoryRequestCommandHandler : IRequestHandler<InformInventoryRequestCommandRequest, InformInventoryRequestCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly InventoryService _inventoryService;

        public InformInventoryRequestCommandHandler(
            RuntimeHandler runtimeHandler,
            InventoryService inventoryService)
        {
            _runtimeHandler = runtimeHandler;
            _inventoryService = inventoryService;
        }

        public async Task<InformInventoryRequestCommandResponse> Handle(InformInventoryRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await InformInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

            await
                _runtimeHandler.ExecuteResultMethod<Task>(
                    _inventoryService,
                    nameof(_inventoryService.InformInventoryRequestAsync),
                    new object[] { request.InventoryRequest, cancellationTokenSource });

            return new InformInventoryRequestCommandResponse();
        }
    }
}
