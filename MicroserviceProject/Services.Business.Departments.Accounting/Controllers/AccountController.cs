using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Business.Departments.Accounting.Models;
using Services.Business.Departments.Accounting.Services;
using Services.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Accounting.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetBankAccountsOfWorker(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<List<BankAccountModel>>(async () =>
            {
                return await _bankService.GetBankAccounts(workerId, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateBankAccount))]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountModel bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateBankAccountValidator.ValidateAsync(bankAccount, cancellationTokenSource);

                return await _bankService.CreateBankAccountAsync(bankAccount, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetCurrencies))]
        public async Task<IActionResult> GetCurrencies(CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<List<CurrencyModel>>(async () =>
            {
                return await _bankService.GetCurrenciesAsync(cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateCurrency))]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyModel currency, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateCurrencyValidator.ValidateAsync(currency, cancellationTokenSource);

                return await _bankService.CreateCurrencyAsync(currency, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpGet]
        [Route(nameof(GetSalaryPaymentsOfWorker))]
        public async Task<IActionResult> GetSalaryPaymentsOfWorker(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<List<SalaryPaymentModel>>(async () =>
            {
                return await _bankService.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationTokenSource);
            },
            services: _bankService);
        }

        [HttpPost]
        [Route(nameof(CreateSalaryPayment))]
        public async Task<IActionResult> CreateSalaryPayment([FromBody] SalaryPaymentModel salaryPayment, CancellationTokenSource cancellationTokenSource)
        {
            SetServiceDefaults(_bankService);

            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await CreateSalaryPaymentValidator.ValidateAsync(salaryPayment, cancellationTokenSource);

                return await _bankService.CreateSalaryPaymentAsync(salaryPayment, cancellationTokenSource);
            },
            services: _bankService);
        }
    }
}