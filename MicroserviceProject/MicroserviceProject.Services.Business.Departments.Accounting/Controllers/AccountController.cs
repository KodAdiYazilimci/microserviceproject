using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Business.Departments.Accounting.Services;
using MicroserviceProject.Services.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment;
using MicroserviceProject.Services.Business.Model.Department.Accounting;

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
    }
}
