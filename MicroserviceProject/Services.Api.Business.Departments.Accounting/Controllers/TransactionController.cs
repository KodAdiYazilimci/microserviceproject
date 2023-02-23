using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Controllers
{
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly BankService _bankService;

        public TransactionController(
            IMediator mediator,
            BankService bankService)
        {
            _mediator = mediator;
            _bankService = bankService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] AccountingRollbackTransactionCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                AccountingRollbackTransactionCommandResponse mediatorResult = await _mediator.Send(request);

                return mediatorResult.Result;
            },
            services: _bankService);
        }
    }
}