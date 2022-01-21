using MediatR;

using Services.Api.Business.Departments.CR.Services;
using Services.Api.Business.Departments.CR.Util.Validation.Customer.CreateCustomer;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
    {
        private readonly CustomerService _customerService;

        public CreateCustomerCommandHandler(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateCustomerValidator.ValidateAsync(request.Customer, cancellationTokenSource);

            return new CreateCustomerCommandResponse()
            {
                CreatedCustomerId = await _customerService.CreateCustomerAsync(request.Customer, cancellationTokenSource)
            };
        }
    }
}
