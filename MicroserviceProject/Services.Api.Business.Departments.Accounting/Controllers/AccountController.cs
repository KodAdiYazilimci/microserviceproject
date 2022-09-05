using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Controllers
{
    [Route("BankAccounts")]
    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly BankService _bankService;

        public AccountController(
            IMediator mediator,
            BankService bankService)
        {
            _mediator = mediator;
            _bankService = bankService;
        }

        [HttpGet]
        [Route(nameof(GetBankAccountsOfWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetBankAccountsOfWorker(int workerId)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetBankAccountsOfWorkerQueryResponse mediatorResult = 
                    await _mediator.Send(new GetBankAccountsOfWorkerQueryRequest() { WorkerId = workerId });

                return mediatorResult.BankAccounts;
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateBankAccount))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetCurrencies))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetCurrencies()
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetCurrenciesQueryResponse mediatorResult = await _mediator.Send(new GetCurrenciesQueryRequest());

                return mediatorResult;
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateCurrency))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetSalaryPaymentsOfWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetSalaryPaymentsOfWorker(int workerId)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                GetSalaryPaymentsOfWorkerQueryResponse mediatorResult = 
                    await _mediator.Send(new GetSalaryPaymentsOfWorkerQueryRequest() { WorkerId = workerId });

                return mediatorResult.SalaryPayments;
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateSalaryPayment))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateSalaryPayment([FromBody] CreateSalaryPaymentCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                await _mediator.Send(request);
            },
            services: _bankService);
        }
    }
}