using MediatR;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests
{
    public class GetBankAccountsOfWorkerQueryRequest : IRequest<GetBankAccountsOfWorkerQueryResponse>
    {
        public int WorkerId { get; set; }
    }
}
