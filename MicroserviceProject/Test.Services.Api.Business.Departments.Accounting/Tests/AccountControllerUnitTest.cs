using Infrastructure.Communication.Http.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.AA;
using Test.Services.Api.Business.Departments.HR;

namespace Test.Services.Api.Business.Departments.Accounting.Tests
{
    [TestClass]
    public class AccountControllerUnitTest : BaseTest
    {
        private AccountControllerTest accountControllerTest = new AccountControllerTest();
        private PersonControllerTest personControllerTest = new PersonControllerTest();

        public AccountControllerUnitTest(InventoryControllerTest inventoryControllerTest, PersonControllerTest personControllerTest, DepartmentControllerTest departmentControllerTest, AccountControllerTest accountControllerTest) : base(inventoryControllerTest, personControllerTest, departmentControllerTest, accountControllerTest)
        {
        }

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public async Task GetBankAccountsOfWorkerTest()
        {
            List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel> workers = await GetWorkersAsync();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            var bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(randomWorkerId);

            if (bankAccounts != null && !bankAccounts.Any())
            {
                await CreateBankAccountToWorker(randomWorkerId);
            }

            Assert.IsTrue(bankAccounts != null && bankAccounts.Any());
        }

        [TestMethod]
        public async Task CreateBankAccountTest()
        {
            var workers = await GetWorkersAsync();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            ServiceResultModel result = await CreateBankAccountToWorker(randomWorkerId);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        public async Task GetCurrenciesTest()
        {
            var currencies = await accountControllerTest.GetCurrenciesAsync();

            if (currencies != null && !currencies.Any())
            {
                await CreateCurrencyTest();

                currencies = await accountControllerTest.GetCurrenciesAsync();
            }

            Assert.IsTrue(currencies != null && currencies.Any());
        }

        [TestMethod]
        public async Task CreateCurrencyTest()
        {
            var result = await accountControllerTest.CreateCurrencyAsync(new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateCurrencyCommandRequest()
            {
                Currency = new global::Services.Communication.Http.Broker.Department.Accounting.Models.CurrencyModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString(),
                    ShortName = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetSalaryPaymentsOfWorkerTest()
        {
            var workers = await GetWorkersAsync();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            var payments = await accountControllerTest.GetSalaryPaymentsOfWorkerAsync(randomWorkerId);

            if (payments != null && !payments.Any())
            {
                await CreateSalaryPaymentToWorker(randomWorkerId);

                payments = await accountControllerTest.GetSalaryPaymentsOfWorkerAsync(randomWorkerId);
            }

            Assert.IsTrue(payments != null && payments.Any());
        }

        [TestMethod]
        public async Task CreateSalaryPaymentTest()
        {
            var workers = await GetWorkersAsync();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            var result = await CreateSalaryPaymentToWorker(randomWorkerId);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestCleanup]
        public void CleanUp()
        {
            accountControllerTest = null;
            personControllerTest = null;
        }
    }
}
