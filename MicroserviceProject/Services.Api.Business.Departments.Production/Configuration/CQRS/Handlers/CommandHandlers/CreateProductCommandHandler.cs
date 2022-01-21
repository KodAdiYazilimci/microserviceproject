using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Product;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly ProductService _productService;

        public CreateProductCommandHandler(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateProductValidator.ValidateAsync(request.Product, cancellationTokenSource);

            return new CreateProductCommandResponse()
            {
                CreatedProductId = await _productService.CreateProductAsync(request.Product, cancellationTokenSource)
            };
        }
    }
}
