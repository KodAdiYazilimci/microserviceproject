using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class ValidateCostInventoryCommandHandler : IRequestHandler<ValidateCostInventoryCommandRequest, ValidateCostInventoryCommandResponse>
    {
        private readonly RequestService _requestService;

        public ValidateCostInventoryCommandHandler(RequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<ValidateCostInventoryCommandResponse> Handle(ValidateCostInventoryCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await ValidateCostInventoryValidator.ValidateAsync(request.DecidedCost, cancellationTokenSource);

            return new ValidateCostInventoryCommandResponse()
            {
                Result = await _requestService.ValidateCostInventoryAsync(request.DecidedCost, cancellationTokenSource)
            };
        }
    }
}
