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
        private readonly RuntimeHandler _runtimeHandler;
        private readonly StockService _stockService;

        public GetStockQueryHandler(
            RuntimeHandler runtimeHandler,
            StockService stockService)
        {
            _runtimeHandler = runtimeHandler;
            _stockService = stockService;
        }

        public async Task<GetStockQueryResponse> Handle(GetStockQueryRequest request, CancellationToken cancellationToken)
        {
            StockModel stock = await _runtimeHandler.ExecuteResultMethod<Task<StockModel>>(
                _stockService,
                nameof(_stockService.GetStockAsync),
                new object[] { request.ProductId, new CancellationTokenSource() });

            return new GetStockQueryResponse()
            {
                Stock = stock
            };
        }
    }
}
