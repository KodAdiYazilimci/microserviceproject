using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.CR.Services;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.CR.CQRS.Commands.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Controllers
{
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly CustomerService _customerService;

        public TransactionController(
            IMediator mediator,
            CustomerService customerService)
        {
            _mediator = mediator;
            _customerService = customerService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackTransactionCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                RollbackTransactionCommandResponse mediatorResult = await _mediator.Send(request);

                return mediatorResult.Result;
            },
            services: _customerService);
        }
    }
}
