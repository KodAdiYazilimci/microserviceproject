using MediatR;

using Services.Api.Business.Departments.Buying.Services;
using Services.Api.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Configuration.CQRS.Handlers.CommandHandlers
{
    public class ValidateCostInventoryCommandHandler : IRequestHandler<ValidateCostInventoryCommandRequest, ValidateCostInventoryCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly RequestService _requestService;
        private readonly ValidateCostInventoryValidator _validateCostInventoryValidator;

        public ValidateCostInventoryCommandHandler(
            RuntimeHandler runtimeHandler,
            RequestService requestService,
            ValidateCostInventoryValidator validateCostInventoryValidator)
        {
            _runtimeHandler = runtimeHandler;
            _requestService = requestService;
            _validateCostInventoryValidator = validateCostInventoryValidator;
        }

        public async Task<ValidateCostInventoryCommandResponse> Handle(ValidateCostInventoryCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _validateCostInventoryValidator.ValidateAsync(request.DecidedCost, cancellationTokenSource);

            return new ValidateCostInventoryCommandResponse()
            {
                Result =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _requestService,
                    nameof(_requestService.ValidateCostInventoryAsync),
                    new object[] { request.DecidedCost, cancellationTokenSource })
            };
        }
    }
}
