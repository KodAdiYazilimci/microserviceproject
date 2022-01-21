using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateSalaryPaymentCommandHandler : IRequestHandler<CreateSalaryPaymentCommandRequest, CreateSalaryPaymentCommandResponse>
    {
        private readonly BankService _bankService;

        public CreateSalaryPaymentCommandHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<CreateSalaryPaymentCommandResponse> Handle(CreateSalaryPaymentCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateSalaryPaymentValidator.ValidateAsync(request.SalaryPayment, cancellationTokenSource);

            return new CreateSalaryPaymentCommandResponse()
            {
                CreatedSalaryPaymentId = await _bankService.CreateSalaryPaymentAsync(request.SalaryPayment, cancellationTokenSource)
            };
        }
    }
}
