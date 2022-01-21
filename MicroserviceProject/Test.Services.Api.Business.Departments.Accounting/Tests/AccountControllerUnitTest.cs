using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.Accounting;
using Services.Api.Business.Departments.Accounting.Controllers;
using Services.Communication.Http.Broker.Department.Accounting.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.Accounting.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.Accounting.Factories.Services;

namespace Test.Services.Api.Business.Departments.Accounting.Tests
{
    [TestClass]
    public class AccountControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private AccountController accountController = null;
        private RouteNameProvider routeNameProvider = null;
        private ServiceCommunicator serviceCommunicator = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            accountController = new AccountController(MediatorFactory.GetInstance(typeof(Startup)), BankServiceFactory.Instance);
            routeNameProvider = RouteNameProviderFactory.GetRouteNameProvider(ConfigurationFactory.GetConfiguration());

            serviceCommunicator =
                ServiceCommunicatorFactory.GetServiceCommunicator(
                    cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                    credentialProvider: CredentialProviderFactory.GetCredentialProvider(ConfigurationFactory.GetConfiguration()),
                    routeNameProvider: routeNameProvider,
                    serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(ConfigurationFactory.GetConfiguration()));
        }

        [TestMethod]
        public async Task GetBankAccountsOfWorkerTest()
        {
            ServiceResultModel<List<WorkerModel>> workersResult =
                await serviceCommunicator.Call<List<WorkerModel>>(
                    serviceName: routeNameProvider.HR_GetWorkers,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult getBankAccountsOfWorkerResult =
                await accountController.GetBankAccountsOfWorker(
                    workerId: workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1)).Id);

            Assert.IsInstanceOfType(getBankAccountsOfWorkerResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateBankAccountTest()
        {
            ServiceResultModel<List<WorkerModel>> workersResult =
                await serviceCommunicator.Call<List<WorkerModel>>(
                    serviceName: routeNameProvider.HR_GetWorkers,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult createBankAccountResult =
                await accountController.CreateBankAccount(
                    new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateBankAccountCommandRequest()
                    {
                        BankAccount = new BankAccountModel
                        {
                            IBAN = new Random().Next(int.MaxValue - 100, int.MaxValue).ToString(),
                            Worker = new WorkerModel()
                            {
                                Id = workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1)).Id
                            }
                        }
                    });

            Assert.IsInstanceOfType(createBankAccountResult, typeof(OkObjectResult));
        }

        public async Task GetCurrenciesTest()
        {
            IActionResult getCurrenciesResult =
                await accountController.GetCurrencies();

            Assert.IsInstanceOfType(getCurrenciesResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateCurrencyTest()
        {
            IActionResult createCurrencyResult =
                await accountController.CreateCurrency(
                    new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateCurrencyCommandRequest()
                    {
                        Currency = new CurrencyModel()
                        {
                            Name = new Random().Next(int.MaxValue / 2, int.MaxValue).ToString(),
                            ShortName = new Random().Next(int.MaxValue / 2, int.MaxValue).ToString()
                        }
                    });

            Assert.IsInstanceOfType(createCurrencyResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetSalaryPaymentsOfWorkerTest()
        {
            ServiceResultModel<List<WorkerModel>> workersResult =
                   await serviceCommunicator.Call<List<WorkerModel>>(
                       serviceName: routeNameProvider.HR_GetWorkers,
                       postData: null,
                       queryParameters: null,
                       headers: null,
                       cancellationTokenSource: cancellationTokenSource);

            IActionResult getSalaryPaymentsOfWorkerResult =
                await accountController.GetSalaryPaymentsOfWorker(
                    workerId: workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1)).Id);

            Assert.IsInstanceOfType(getSalaryPaymentsOfWorkerResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateSalaryPaymentTest()
        {
            ServiceResultModel<List<WorkerModel>> workersResult =
                await serviceCommunicator.Call<List<WorkerModel>>(
                    serviceName: routeNameProvider.HR_GetWorkers,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            Task<ServiceResultModel<List<BankAccountModel>>> getBankAccountsTask =
                serviceCommunicator.Call<List<BankAccountModel>>(
                    serviceName: routeNameProvider.Accounting_GetBankAccountsOfWorker,
                    postData: null,
                    queryParameters: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("workerId", workersResult.Data.ElementAt(new Random().Next(0,workersResult.Data.Count-1)).Id.ToString())
                    },
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            Task<ServiceResultModel<List<CurrencyModel>>> getCurrenciesTask =
                            serviceCommunicator.Call<List<CurrencyModel>>(
                            serviceName: routeNameProvider.Accounting_GetCurrencies,
                            postData: null,
                            queryParameters: null,
                            headers: null,
                            cancellationTokenSource: cancellationTokenSource);

            Task.WaitAll(getBankAccountsTask, getCurrenciesTask);

            IActionResult createSalaryPayment =
                await accountController.CreateSalaryPayment(
                    new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateSalaryPaymentCommandRequest()
                    {
                        SalaryPayment = new SalaryPaymentModel()
                        {
                            Amount = new Random().Next(1, short.MaxValue),
                            Date = DateTime.Now,
                            BankAccount = getBankAccountsTask.Result.Data.ElementAt(new Random().Next(0, getBankAccountsTask.Result.Data.Count - 1)),
                            Currency = getCurrenciesTask.Result.Data.ElementAt(new Random().Next(0, getCurrenciesTask.Result.Data.Count - 1))
                        }
                    });

            Assert.IsInstanceOfType(createSalaryPayment, typeof(OkObjectResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            cancellationTokenSource = null;
            accountController.Dispose();
        }
    }
}
