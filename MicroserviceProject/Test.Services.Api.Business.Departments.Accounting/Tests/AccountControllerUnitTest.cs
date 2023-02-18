using Infrastructure.Communication.Http.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.HR;

namespace Test.Services.Api.Business.Departments.Accounting.Tests
{
    [TestClass]
    public class AccountControllerUnitTest
    {
        private AccountControllerTest accountControllerTest;
        private PersonControllerTest personControllerTest;

        [TestInitialize]
        public void Init()
        {
            accountControllerTest = new AccountControllerTest();
            personControllerTest = new PersonControllerTest();
        }

        [TestMethod]
        public async Task GetBankAccountsOfWorkerTest()
        {
            List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel> workers = await GetWorkers();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            var bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(randomWorkerId);

            if (bankAccounts != null && !bankAccounts.Any())
            {
                await CreateBankAccountToWorker(randomWorkerId);
            }

            Assert.IsTrue(bankAccounts != null && bankAccounts.Any());
        }

        private async Task<List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel>> GetWorkers()
        {
            var workers = await personControllerTest.GetWorkersAsync();

            if (workers != null && !workers.Any())
            {
                var people = await personControllerTest.GetPeopleAsync();

                if (people != null && !people.Any())
                {
                    var createPeopleResult = await personControllerTest.CreatePersonAsync(new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreatePersonCommandRequest()
                    {
                        Person = new global::Services.Communication.Http.Broker.Department.HR.Models.PersonModel()
                        {
                            Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                        }
                    });

                    people = await personControllerTest.GetPeopleAsync();
                }

                var createWorkerResult = await personControllerTest.CreateWorkerAsync(new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreateWorkerCommandRequest()
                {
                    Worker = new global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel()
                    {
                        Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                    }
                });

                workers = await personControllerTest.GetWorkersAsync();
            }

            return workers;
        }

        [TestMethod]
        public async Task CreateBankAccountTest()
        {
            var workers = await GetWorkers();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            ServiceResultModel result = await CreateBankAccountToWorker(randomWorkerId);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        private async Task<ServiceResultModel> CreateBankAccountToWorker(int workerId)
        {
            return await accountControllerTest.CreateBankAccountTestAsync(new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateBankAccountCommandRequest()
            {
                BankAccount = new global::Services.Communication.Http.Broker.Department.Accounting.Models.BankAccountModel()
                {
                    IBAN = new Random().Next(int.MinValue, int.MaxValue).ToString(),
                    Worker = new global::Services.Communication.Http.Broker.Department.Accounting.Models.WorkerModel()
                    {
                        Id = workerId,
                    }
                }
            });
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
            var workers = await GetWorkers();

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
            var workers = await GetWorkers();

            var randomWorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id;

            var result = await CreateSalaryPaymentToWorker(randomWorkerId);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        private async Task<ServiceResultModel> CreateSalaryPaymentToWorker(int workerId)
        {
            var bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(workerId);

            var randomBankAccount = bankAccounts.ElementAt(new Random().Next(0, bankAccounts.Count - 1));

            var result = await accountControllerTest.CreateSalaryPaymentTest(new global::Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests.CreateSalaryPaymentCommandRequest()
            {
                SalaryPayment = new global::Services.Communication.Http.Broker.Department.Accounting.Models.SalaryPaymentModel()
                {
                    Amount = new Random().Next(1, byte.MaxValue),
                    Date = DateTime.Now,
                    BankAccount = randomBankAccount
                }
            });
            return result;
        }

        [TestCleanup]
        public void CleanUp()
        {
            accountControllerTest = null;
            personControllerTest = null;
        }
    }
}
