using MediatR;

using Services.Api.Business.Departments.Storage.Services;
using Services.Api.Business.Departments.Storage.Util.Validation.Stock;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateStockCommandHandler : IRequestHandler<CreateStockCommandRequest, CreateStockCommandResponse>
    {
        private readonly StockService _stockService;

        public CreateStockCommandHandler(StockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<CreateStockCommandResponse> Handle(CreateStockCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateStockValidator.ValidateAsync(request.Stock, cancellationTokenSource);

            return new CreateStockCommandResponse()
            {
                CreatedStockId = await _stockService.CreateStockAsync(request.Stock, cancellationTokenSource)
            };
        }
    }
}
