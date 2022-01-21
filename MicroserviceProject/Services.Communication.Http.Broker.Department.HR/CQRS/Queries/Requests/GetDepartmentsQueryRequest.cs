using MediatR;

using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests
{
    public class GetDepartmentsQueryRequest : IRequest<GetDepartmentsQueryResponse>
    {
    }
}
