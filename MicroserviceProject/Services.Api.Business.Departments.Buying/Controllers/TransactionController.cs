using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Buying.Services;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Controllers
{
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly RequestService _inventoryService;

        public TransactionController(
            IMediator mediator,
            RequestService inventoryService)
        {
            _mediator = mediator;
            _inventoryService = inventoryService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackTransactionCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<RollbackTransactionCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _inventoryService);
        }
    }
}
