using Communication.Http.Department.Production.Models;

using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Production.Services;
using Services.Business.Departments.Production.Util.Validation.Production;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Controllers
{
    [Authorize]
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
