using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Services;
using Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.Accounting.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Controllers
{
    [Route("BankAccounts")]
    public class AccountController : BaseController
    {
        private readonly BankService _bankService;

        public AccountController(BankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        [Route(nameof(GetBankAccountsOfWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetBankAccountsOfWorker(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<BankAccountModel>>(async () =>
            {
                return await _bankService.GetBankAccounts(workerId, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateBankAccount))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountModel bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateBankAccountValidator.ValidateAsync(bankAccount, cancellationTokenSource);

                return await _bankService.CreateBankAccountAsync(bankAccount, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetCurrencies))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetCurrencies(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<CurrencyModel>>(async () =>
            {
                return await _bankService.GetCurrenciesAsync(cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateCurrency))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyModel currency, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateCurrencyValidator.ValidateAsync(currency, cancellationTokenSource);

                return await _bankService.CreateCurrencyAsync(currency, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetSalaryPaymentsOfWorker))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetSalaryPaymentsOfWorker(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<SalaryPaymentModel>>(async () =>
            {
                return await _bankService.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateSalaryPayment))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateSalaryPayment([FromBody] SalaryPaymentModel salaryPayment, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateSalaryPaymentValidator.ValidateAsync(salaryPayment, cancellationTokenSource);

                return await _bankService.CreateSalaryPaymentAsync(salaryPayment, cancellationTokenSource);
            },
            services: _bankService);
        }
    }
}