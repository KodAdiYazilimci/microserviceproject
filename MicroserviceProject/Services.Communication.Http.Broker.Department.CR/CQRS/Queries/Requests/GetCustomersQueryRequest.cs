using MediatR;

using Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Requests
{
    public class GetCustomersQueryRequest : IRequest<GetCustomersQueryResponse>
    {
    }
}
