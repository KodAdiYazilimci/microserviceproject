using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Production.Services;
using Services.Api.Business.Departments.Production.Util.Validation.Production;
using Services.Communication.Http.Broker.Department.Production.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Controllers
{
    [Route("Production")]
    public class ProductionController : BaseController
    {
        private readonly ProductionService _productionService;

        public ProductionController(ProductionService productionService)
        {
            _productionService = productionService;
        }

        [HttpPost]
        [Route(nameof(ProduceProduct))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> ProduceProduct([FromBody] ProduceModel produceModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await ProduceProductValidator.ValidateAsync(produceModel, cancellationTokenSource);

                return await _productionService.ProduceProductAsync(produceModel, cancellationTokenSource);
            },
            services: _productionService);
        }

        [HttpPost]
        [Route(nameof(ReEvaluateProduceProduct))]
        [Authorize(Roles = "ApiUser,QueueUser")]
        public async Task<IActionResult> ReEvaluateProduceProduct(int referenceNumber, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await ReEvaluateProduceProductValidator.ValidateAsync(referenceNumber, cancellationTokenSource);

                return await _productionService.ReEvaluateProduceProductAsync(referenceNumber, cancellationTokenSource);
            },
           services: _productionService);
        }
    }
}
