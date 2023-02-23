using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetBankAccountsOfWorkerQueryHandler : IRequestHandler<AccountingGetBankAccountsOfWorkerQueryRequest, AccountingGetBankAccountsOfWorkerQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly BankService _bankService;

        public GetBankAccountsOfWorkerQueryHandler(
            RuntimeHandler runtimeHandler,
            BankService bankService)
        {
            _runtimeHandler = runtimeHandler;
            _bankService = bankService;
        }

        public async Task<AccountingGetBankAccountsOfWorkerQueryResponse> Handle(AccountingGetBankAccountsOfWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return new AccountingGetBankAccountsOfWorkerQueryResponse()
            {
                BankAccounts =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<AccountingBankAccountModel>>>(
                    _bankService,
                    nameof(_bankService.GetBankAccounts),
                    new object[] { request.WorkerId, new CancellationTokenSource() })
            };
        }
    }
}
