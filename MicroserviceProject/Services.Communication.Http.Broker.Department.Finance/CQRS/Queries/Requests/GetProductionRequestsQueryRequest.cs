using MediatR;

using Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Finance.CQRS.Queries.Requests
{
    public class GetProductionRequestsQueryRequest : IRequest<GetProductionRequestsQueryResponse>
    {
    }
}
