using MediatR;

using Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.IT.CQRS.Queries.Requests
{
    public class GetInventoriesQueryRequest : IRequest<GetInventoriesQueryResponse>
    {
    }
}
