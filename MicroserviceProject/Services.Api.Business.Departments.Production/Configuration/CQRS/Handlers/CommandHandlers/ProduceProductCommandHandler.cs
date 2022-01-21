using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Production;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.CommandHandlers
{
    public class ProduceProductCommandHandler : IRequestHandler<ProduceProductCommandRequest, ProduceProductCommandResponse>
    {
        private readonly ProductionService _productionService;

        public ProduceProductCommandHandler(ProductionService productionService)
        {
            _productionService = productionService;
        }

        public async Task<ProduceProductCommandResponse> Handle(ProduceProductCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await ProduceProductValidator.ValidateAsync(request.Produce, cancellationTokenSource);

            return new ProduceProductCommandResponse()
            {
                ProductId = await _productionService.ProduceProductAsync(request.Produce, cancellationTokenSource)
            };
        }
    }
}
