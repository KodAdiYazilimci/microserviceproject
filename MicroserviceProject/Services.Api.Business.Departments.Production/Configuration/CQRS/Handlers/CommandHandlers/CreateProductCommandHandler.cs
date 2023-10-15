using MediatR;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Product;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Responses;
using Services.Logging.Aspect.Handlers;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Configuration.CQRS.Handlers.CommandHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly ProductService _productService;
        private readonly CreateProductValidator _createProductValidator;

        public CreateProductCommandHandler(
            RuntimeHandler runtimeHandler,
            ProductService productService,
            CreateProductValidator createProductValidator)
        {
            _runtimeHandler = runtimeHandler;
            _productService = productService;
            _createProductValidator = createProductValidator;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _createProductValidator.ValidateAsync(request.Product, cancellationTokenSource);

            return new CreateProductCommandResponse()
            {
                CreatedProductId =
                await
                _runtimeHandler.ExecuteResultMethod<Task<int>>(
                    _productService,
                    nameof(_productService.CreateProductAsync),
                    new object[] { request.Product, cancellationTokenSource })
            };
        }
    }
}
