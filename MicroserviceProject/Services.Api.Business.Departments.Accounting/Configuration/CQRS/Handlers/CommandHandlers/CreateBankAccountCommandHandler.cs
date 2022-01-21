using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommandRequest, CreateBankAccountCommandResponse>
    {
        private readonly BankService _bankService;

        public CreateBankAccountCommandHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<CreateBankAccountCommandResponse> Handle(CreateBankAccountCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateBankAccountValidator.ValidateAsync(request.BankAccount, cancellationTokenSource);

            return new CreateBankAccountCommandResponse()
            {
                CreatedBankAccountId = await _bankService.CreateBankAccountAsync(request.BankAccount, cancellationTokenSource)
            };
        }
    }
}
