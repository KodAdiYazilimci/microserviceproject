using MediatR;

using Services.Api.Business.Departments.Storage.Services;
using Services.Api.Business.Departments.Storage.Util.Validation.Stock;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Configuration.CQRS.Handlers.CommandHandlers
{
    public class DescendProductStockCommandHandler : IRequestHandler<DescendProductStockCommandRequest, DescendProductStockCommandResponse>
    {
        private readonly StockService _stockService;

        public DescendProductStockCommandHandler(StockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<DescendProductStockCommandResponse> Handle(DescendProductStockCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await DescendStockValidator.ValidateAsync(request.Stock, cancellationTokenSource);

            return new DescendProductStockCommandResponse()
            {
                StockId = await _stockService.DescendProductStockAsync(request.Stock, cancellationTokenSource)
            };
        }
    }
}
