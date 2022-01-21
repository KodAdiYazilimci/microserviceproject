using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommandRequest, CreateCurrencyCommandResponse>
    {
        private readonly BankService _bankService;

        public CreateCurrencyCommandHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<CreateCurrencyCommandResponse> Handle(CreateCurrencyCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateCurrencyValidator.ValidateAsync(request.Currency, cancellationTokenSource);

            return new CreateCurrencyCommandResponse()
            {
                CreatedCurrencyId =
                await _bankService.CreateCurrencyAsync(request.Currency, cancellationTokenSource)
            };
        }
    }
}
