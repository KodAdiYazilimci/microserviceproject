using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

using System.Threading;
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
                if (ByPassMediatR)
                    return await _bankService.GetBankAccounts(workerId, new CancellationTokenSource());
                else
                    return (await _mediator.Send(new AccountingGetBankAccountsOfWorkerQueryRequest() { WorkerId = workerId })).BankAccounts;
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateBankAccount))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateBankAccount([FromBody] AccountingCreateBankAccountCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _bankService.CreateBankAccountAsync(request.BankAccount, new CancellationTokenSource());
                else
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
                if (ByPassMediatR)
                    return await _bankService.GetCurrenciesAsync(new CancellationTokenSource());
                else
                {
                    return (await _mediator.Send(new AccountingGetCurrenciesQueryRequest())).Currencies;
                }
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateCurrency))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateCurrency([FromBody] AccountingCreateCurrencyCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _bankService.CreateCurrencyAsync(request.Currency, new CancellationTokenSource());
                else
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
                if (ByPassMediatR)
                    return await _bankService.GetSalaryPaymentsOfWorkerAsync(workerId, new CancellationTokenSource());
                else
                    return (await _mediator.Send(new AccountingGetSalaryPaymentsOfWorkerQueryRequest() { WorkerId = workerId })).SalaryPayments;
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateSalaryPayment))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateSalaryPayment([FromBody] AccountingCreateSalaryPaymentCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                if (ByPassMediatR)
                    await _bankService.CreateSalaryPaymentAsync(request.SalaryPayment, new CancellationTokenSource());
                else
                    await _mediator.Send(request);
            },
            services: _bankService);
        }
    }
}