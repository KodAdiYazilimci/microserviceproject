using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Controllers;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.Models;

using Test.Services.Api.Business.Departments.Accounting.Factories.Services;

namespace Test.Services.Api.Business.Departments.Accounting
{
    public class AccountControllerTest
    {
        private readonly AccountController accountController;

        public AccountControllerTest()
        {
            accountController = new AccountController(null, BankServiceFactory.Instance);
            accountController.ByPassMediatR = true;
        }

        public async Task<List<AccountingBankAccountModel>> GetBankAccountsOfWorkerAsync(int workerId)
        {
            IActionResult actionResult = await accountController.GetBankAccountsOfWorker(workerId);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                var inventories = okObjectResult.Value as ServiceResultModel<List<AccountingBankAccountModel>>;

                return inventories.Data;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateBankAccountTestAsync(AccountingCreateBankAccountCommandRequest createBankAccountCommandRequest)
        {
            IActionResult actionResult = await accountController.CreateBankAccount(createBankAccountCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<List<AccountingCurrencyModel>> GetCurrenciesAsync()
        {
            IActionResult actionResult = await accountController.GetCurrencies();

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return (okObjectResult.Value as ServiceResultModel<List<AccountingCurrencyModel>>).Data;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateCurrencyAsync(AccountingCreateCurrencyCommandRequest createCurrencyCommandRequest)
        {
            IActionResult actionResult = await accountController.CreateCurrency(createCurrencyCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<List<AccountingSalaryPaymentModel>> GetSalaryPaymentsOfWorkerAsync(int workerId)
        {
            IActionResult actionResult = await accountController.GetSalaryPaymentsOfWorker(workerId);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return (okObjectResult.Value as ServiceResultModel<List<AccountingSalaryPaymentModel>>).Data;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentTest(AccountingCreateSalaryPaymentCommandRequest createSalaryPaymentCommandRequest)
        {
            IActionResult actionResult = await accountController.CreateSalaryPayment(createSalaryPaymentCommandRequest);

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                return okObjectResult.Value as ServiceResultModel;
            }
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return null;
        }
    }
}
