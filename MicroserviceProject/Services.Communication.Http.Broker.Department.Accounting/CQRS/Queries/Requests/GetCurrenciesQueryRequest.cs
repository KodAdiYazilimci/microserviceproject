using MediatR;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests
{
    public class GetCurrenciesQueryRequest : IRequest<GetCurrenciesQueryResponse>
    {
    }
}
