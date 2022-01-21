using MediatR;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetBankAccountsOfWorkerQueryHandler : IRequestHandler<GetBankAccountsOfWorkerQueryRequest, GetBankAccountsOfWorkerQueryResponse>
    {
        private readonly BankService _bankService;

        public GetBankAccountsOfWorkerQueryHandler(BankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<GetBankAccountsOfWorkerQueryResponse> Handle(GetBankAccountsOfWorkerQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetBankAccountsOfWorkerQueryResponse()
            {
                BankAccounts = await _bankService.GetBankAccounts(request.WorkerId, new CancellationTokenSource())
            };
        }
    }
}
