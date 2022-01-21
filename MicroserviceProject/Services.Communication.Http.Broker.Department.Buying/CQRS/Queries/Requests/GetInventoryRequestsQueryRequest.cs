using MediatR;

using Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Buying.CQRS.Queries.Requests
{
    public class GetInventoryRequestsQueryRequest : IRequest<GetInventoryRequestsQueryResponse>
    {
    }
}
