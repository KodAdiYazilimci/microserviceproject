using MediatR;

using Services.Api.Business.Departments.Storage.Services;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetStockQueryHandler : IRequestHandler<GetStockQueryRequest, GetStockQueryResponse>
    {
        private readonly StockService _stockService;

        public GetStockQueryHandler(StockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<GetStockQueryResponse> Handle(GetStockQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetStockQueryResponse()
            {
                Stock = await _stockService.GetStockAsync(request.ProductId, new CancellationTokenSource())
            };
        }
    }
}
