using MediatR;

using Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.AA.CQRS.Queries.Requests
{
    public class GetInventoriesQueryRequest : IRequest<GetInventoriesQueryResponse>
    {
    }
}
