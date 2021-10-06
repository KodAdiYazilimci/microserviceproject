using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Production.Models;
using Services.Business.Departments.Production.Services;
using Services.Business.Departments.Production.Util.Validation.Product;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Controllers
{
    [Authorize]
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
