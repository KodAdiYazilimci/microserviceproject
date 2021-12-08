using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Storage.Services;
using Services.Api.Business.Departments.Storage.Util.Validation.Stock;
using Services.Communication.Http.Broker.Department.Storage.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Controllers
{
    [Route("Stock")]
    public class StockController : BaseController
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        [Route(nameof(GetStock))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetStock(int productId, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<StockModel>(async () =>
            {
                return await _stockService.GetStockAsync(productId, cancellationTokenSource);
            },
            services: _stockService);
        }

        [HttpPost]
        [Route(nameof(CreateStock))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateStock([FromBody] StockModel stockModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateStockValidator.ValidateAsync(stockModel, cancellationTokenSource);

                return await _stockService.CreateStockAsync(stockModel, cancellationTokenSource);
            },
            services: _stockService);
        }

        [HttpPost]
        [Route(nameof(DescendProductStock))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> DescendProductStock([FromBody] StockModel stockModel,CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await DescendStockValidator.ValidateAsync(stockModel, cancellationTokenSource);

                return await _stockService.DescendProductStockAsync(stockModel, cancellationTokenSource);
            },
            services: _stockService);
        }
    }
}
