using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Product;
using Services.Communication.Http.Broker.Department.Production.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Controllers
{
    [Route("Product")]
    public class ProductController : BaseController
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route(nameof(GetProducts))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetProducts(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<ProductModel>>(async () =>
            {
                return await _productService.GetProductsAsync(cancellationTokenSource);
            },
            services: _productService);
        }

        [HttpPost]
        [Route(nameof(CreateProduct))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel productModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateProductValidator.ValidateAsync(productModel, cancellationTokenSource);

                return await _productService.CreateProductAsync(productModel, cancellationTokenSource);
            },
            services: _productService);
        }
    }
}
