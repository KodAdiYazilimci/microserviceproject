using MediatR;

using Services.Api.Business.Departments.CR.Services;
using Services.Api.Business.Departments.CR.Util.Validation.Customer.CreateCustomer;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly CustomerService _customerService;
        private readonly CreateCustomerValidator _createCustomerValidator;

        public CreateCustomerCommandHandler(
            RuntimeHandler runtimeHandler,
            CustomerService customerService,
            CreateCustomerValidator createCustomerValidator)
        {
            _runtimeHandler = runtimeHandler;
            _customerService = customerService;
            _createCustomerValidator = createCustomerValidator;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createCustomerValidator.ValidateAsync(request.Customer, cancellationTokenSource);

            return new CreateCustomerCommandResponse()
            {
                CreatedCustomerId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _customerService,
                    nameof(_customerService.CreateCustomerAsync),
                    new object[] { request.Customer, cancellationTokenSource })
            };
        }
    }
}
