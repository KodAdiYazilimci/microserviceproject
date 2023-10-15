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
    public class CreateCurrencyCommandHandler : IRequestHandler<AccountingCreateCurrencyCommandRequest, AccountingCreateCurrencyCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;
        private readonly CreateCurrencyValidator _createCurrencyValidator;

        public CreateCurrencyCommandHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService,
            CreateCurrencyValidator createCurrencyValidator)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
            _createCurrencyValidator = createCurrencyValidator;
        }

        public async Task<AccountingCreateCurrencyCommandResponse> Handle(AccountingCreateCurrencyCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createCurrencyValidator.ValidateAsync(request.Currency, cancellationTokenSource);

            return new AccountingCreateCurrencyCommandResponse()
            {
                CreatedCurrencyId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _bankService,
                    nameof(_bankService.CreateCurrencyAsync),
                    new object[] { request.Currency, cancellationTokenSource })
            };
        }
    }
}
