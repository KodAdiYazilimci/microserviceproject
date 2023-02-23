using MediatR;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests
{
    public class AccountingGetBankAccountsOfWorkerQueryRequest : IRequest<AccountingGetBankAccountsOfWorkerQueryResponse>
    {
        public int WorkerId { get; set; }
    }
}
