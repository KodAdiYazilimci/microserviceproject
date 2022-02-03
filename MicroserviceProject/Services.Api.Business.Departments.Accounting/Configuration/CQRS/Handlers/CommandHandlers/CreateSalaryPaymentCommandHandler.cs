using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateSalaryPaymentCommandHandler : IRequestHandler<CreateSalaryPaymentCommandRequest, CreateSalaryPaymentCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;

        public CreateSalaryPaymentCommandHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
        }

        public async Task<CreateSalaryPaymentCommandResponse> Handle(CreateSalaryPaymentCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateSalaryPaymentValidator.ValidateAsync(request.SalaryPayment, cancellationTokenSource);

            return new CreateSalaryPaymentCommandResponse()
            {
                CreatedSalaryPaymentId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _bankService,
                    nameof(_bankService.CreateSalaryPaymentAsync),
                    new object[] { request.SalaryPayment, cancellationTokenSource })
            };
        }
    }
}
