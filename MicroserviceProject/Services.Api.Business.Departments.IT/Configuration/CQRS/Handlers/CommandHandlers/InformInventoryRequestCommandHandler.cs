using MediatR;

using Services.Api.Business.Departments.IT.Services;
using Services.Api.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.IT.Configuration.CQRS.Handlers.CommandHandlers
{
    public class InformInventoryRequestCommandHandler : IRequestHandler<InformInventoryRequestCommandRequest, InformInventoryRequestCommandResponse>
    {
        private readonly InventoryService _inventoryService;

        public InformInventoryRequestCommandHandler(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<InformInventoryRequestCommandResponse> Handle(InformInventoryRequestCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await InformInventoryRequestValidator.ValidateAsync(request.InventoryRequest, cancellationTokenSource);

            await _inventoryService.InformInventoryRequestAsync(request.InventoryRequest, cancellationTokenSource);

            return new InformInventoryRequestCommandResponse();
        }
    }
}
