
using MicroserviceProject.Services.Business.Departments.Buying.Services;
using MicroserviceProject.Services.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest;
using MicroserviceProject.Services.Model.Department.Buying;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Buying.Controllers
{
    [Authorize]
    [Route("Request")]
    public class RequestController : Controller
    {
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Route(nameof(GetInventoryRequests))]
        public async Task<IActionResult> GetInventoryRequests(CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _requestService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<List<InventoryRequestModel>>(async () =>
            {
                return await _requestService.GetInventoryRequestsAsync(cancellationToken);
            },
            services: _requestService);
        }

        [HttpPost]
        [Route(nameof(CreateInventoryRequest))]
        public async Task<IActionResult> CreateInventoryRequest([FromBody] InventoryRequestModel requestModel, CancellationToken cancellationToken)
        {
            if (Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _requestService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await CreateInventoryRequestValidator.ValidateAsync(requestModel, cancellationToken);

                return await _requestService.CreateInventoryRequestAsync(requestModel, cancellationToken);
            },
            services: _requestService);
        }
    }
}
