using MediatR;

using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Requests
{
    public class GetStockQueryRequest : IRequest<GetStockQueryResponse>
    {
        public int ProductId { get; set; }
    }
}
