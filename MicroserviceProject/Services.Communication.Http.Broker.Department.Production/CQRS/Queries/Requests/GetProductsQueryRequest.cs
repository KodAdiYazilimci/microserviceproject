using MediatR;

using Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Production.CQRS.Queries.Requests
{
    public class GetProductsQueryRequest:IRequest<GetProductsQueryResponse>
    {
    }
}
