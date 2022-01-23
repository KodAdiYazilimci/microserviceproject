using MediatR;

using Services.Api.Business.Departments.Storage.Services;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Storage.Models;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetStockQueryHandler : IRequestHandler<GetStockQueryRequest, GetStockQueryResponse>
    {
        private readonly StockService _stockService;
        private readonly RuntimeLogHandler _runtimeLogHandler;

        public GetStockQueryHandler(
            RuntimeLogHandler runtimeLogHandler,
            StockService stockService)
        {
            _runtimeLogHandler = runtimeLogHandler;
            _stockService = stockService;
        }

        public async Task<GetStockQueryResponse> Handle(GetStockQueryRequest request, CancellationToken cancellationToken)
        {
            StockModel stock = await _runtimeLogHandler.ExecuteMethod<int, CancellationTokenSource, Task<StockModel>>(
                _stockService.GetStockAsync,
                new object[] { request.ProductId, new CancellationTokenSource() });

            //StockModel stock = await _stockService.GetStockAsync(request.ProductId, new CancellationTokenSource());

            return new GetStockQueryResponse()
            {
                Stock = stock
            };
        }
    }
}
