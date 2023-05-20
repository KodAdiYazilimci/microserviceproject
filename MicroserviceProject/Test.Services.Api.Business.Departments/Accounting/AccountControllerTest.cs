using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.CommandHandlers;
using Services.Api.Business.Departments.Accounting.Configuration.CQRS.Handlers.QueryHandlers;
using Services.Api.Business.Departments.Accounting.Controllers;
using Services.Api.Business.Departments.Accounting.Services;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Logging.Aspect.Handlers;
using Services.Runtime.Aspect.Mock;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.Accounting.Factories.Services;

namespace Test.Services.Api.Business.Departments.Accounting
{
    public class AccountControllerTest
    {
        private readonly RuntimeHandler runtimeHandler;
        private readonly BankService bankService;
        private readonly AccountController accountController;

        public AccountControllerTest()
        {
            runtimeHandler = RuntimeHandlerFactory.GetInstance(
                runtimeLogger: RuntimeLoggerFactory.GetInstance(
                    configuration: ConfigurationFactory.GetConfiguration()));

            bankService = BankServiceFactory.Instance;
            accountController = new AccountController(null, bankService);
            accountController.ByPassMediatR = true;
        }

        public async Task<List<AccountingBankAccountModel>> GetBankAccountsOfWorkerAsync(int workerId, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingGetBankAccountsOfWorkerQueryRequest, AccountingGetBankAccountsOfWorkerQueryResponse>(
                    request: new AccountingGetBankAccountsOfWorkerQueryRequest() { WorkerId = workerId },
                    requestHandler: new GetBankAccountsOfWorkerQueryHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return response.BankAccounts;
            }
        }

        public async Task<ServiceResultModel> CreateBankAccountTestAsync(AccountingCreateBankAccountCommandRequest createBankAccountCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingCreateBankAccountCommandRequest, AccountingCreateBankAccountCommandResponse>(
                    request: createBankAccountCommandRequest,
                    requestHandler: new CreateBankAccountCommandHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return new ServiceResultModel() { IsSuccess = response.CreatedBankAccountId > 0 };
            }
        }

        public async Task<List<AccountingCurrencyModel>> GetCurrenciesAsync(bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingGetCurrenciesQueryRequest, AccountingGetCurrenciesQueryResponse>(
                    request: new AccountingGetCurrenciesQueryRequest(),
                    requestHandler: new GetCurrenciesQueryHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return response.Currencies;
            }
        }

        public async Task<ServiceResultModel> CreateCurrencyAsync(AccountingCreateCurrencyCommandRequest createCurrencyCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingCreateCurrencyCommandRequest, AccountingCreateCurrencyCommandResponse>(
                    request: createCurrencyCommandRequest,
                    requestHandler: new CreateCurrencyCommandHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return new ServiceResultModel() { IsSuccess = response.CreatedCurrencyId > 0 };
            }
        }

        public async Task<List<AccountingSalaryPaymentModel>> GetSalaryPaymentsOfWorkerAsync(int workerId, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingGetSalaryPaymentsOfWorkerQueryRequest, AccountingGetSalaryPaymentsOfWorkerQueryResponse>(
                    request: new AccountingGetSalaryPaymentsOfWorkerQueryRequest() { WorkerId = workerId },
                    requestHandler: new GetSalaryPaymentsOfWorkerQueryHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return response.SalaryPayments;
            }
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentTest(AccountingCreateSalaryPaymentCommandRequest createSalaryPaymentCommandRequest, bool byPassMediatR = true)
        {
            if (byPassMediatR)
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
            else
            {
                var response = MediatorFactory.GetInstance<AccountingCreateSalaryPaymentCommandRequest, AccountingCreateSalaryPaymentCommandResponse>(
                    request: createSalaryPaymentCommandRequest,
                    requestHandler: new CreateSalaryPaymentCommandHandler(
                        runtimeHandler: runtimeHandler,
                        bankService: bankService));

                return new ServiceResultModel() { IsSuccess = response.CreatedSalaryPaymentId > 0 };
            }
        }
    }
}
