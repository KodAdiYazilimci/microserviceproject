using MediatR;

using Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Selling.CQRS.Queries.Requests
{
    public class GetSoldsQueryRequest : IRequest<GetSoldsQueryResponse>
    {
    }
}
