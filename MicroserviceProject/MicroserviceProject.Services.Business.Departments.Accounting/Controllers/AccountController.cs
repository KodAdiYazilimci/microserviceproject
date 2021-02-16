using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Business.Departments.Accounting.Services;
using MicroserviceProject.Services.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using MicroserviceProject.Services.Model.Department.Accounting;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Controllers
{
    [Authorize]
    [Route("BankAccounts")]
    public class AccountController : Controller
    {
        private readonly BankService _bankService;

        public AccountController(BankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        [Route(nameof(GetBankAccountsOfWorker))]
        public async Task<IActionResult> GetBankAccountsOfWorker(int workerId, CancellationToken cancellationToken)
        {
            try
            {
                List<BankAccountModel> bankAccounts =
                    await _bankService.GetBankAccounts(workerId, cancellationToken);

                ServiceResult<List<BankAccountModel>> serviceResult = new ServiceResult<List<BankAccountModel>>()
                {
                    IsSuccess = true,
                    Data = bankAccounts
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateBankAccount))]
        public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountModel bankAccount, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateBankAccountValidator.ValidateAsync(bankAccount, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _bankService.CreateBankAccountAsync(bankAccount, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpGet]
        [Route(nameof(GetCurrencies))]
        public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken)
        {
            try
            {
                List<CurrencyModel> currencies =
                    await _bankService.GetCurrenciesAsync(cancellationToken);

                ServiceResult<List<CurrencyModel>> serviceResult = new ServiceResult<List<CurrencyModel>>()
                {
                    IsSuccess = true,
                    Data = currencies
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateCurrency))]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyModel currency, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateCurrencyValidator.ValidateAsync(currency, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _bankService.CreateCurrencyAsync(currency, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpGet]
        [Route(nameof(GetSalaryPaymentsOfWorker))]
        public async Task<IActionResult> GetSalaryPaymentsOfWorker(int workerId, CancellationToken cancellationToken)
        {
            try
            {
                List<SalaryPaymentModel> salaryPayments =
                    await _bankService.GetSalaryPaymentsOfWorkerAsync(workerId, cancellationToken);

                ServiceResult<List<SalaryPaymentModel>> serviceResult = new ServiceResult<List<SalaryPaymentModel>>()
                {
                    IsSuccess = true,
                    Data = salaryPayments
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }

        [HttpPost]
        [Route(nameof(CreateSalaryPayment))]
        public async Task<IActionResult> CreateSalaryPayment([FromBody] SalaryPaymentModel salaryPayment, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult =
                    await CreateSalaryPaymentValidator.ValidateAsync(salaryPayment, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                int generatedId = await _bankService.CreateSalaryPaymentAsync(salaryPayment, cancellationToken);

                ServiceResult<int> serviceResult = new ServiceResult<int>()
                {
                    IsSuccess = true,
                    Data = generatedId
                };

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }
    }
}