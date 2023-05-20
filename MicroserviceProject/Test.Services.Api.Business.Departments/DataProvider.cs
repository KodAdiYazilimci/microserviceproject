using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.Models;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using Test.Services.Api.Business.Departments.AA;
using Test.Services.Api.Business.Departments.Accounting;
using Test.Services.Api.Business.Departments.HR;

namespace Test.Services.Api.Business.Departments
{
    public class DataProvider
    {
        private AccountControllerTest accountControllerTest;
        private DepartmentControllerTest departmentControllerTest;
        private InventoryControllerTest inventoryControllerTest;
        private PersonControllerTest personControllerTest;

        public DataProvider(InventoryControllerTest inventoryControllerTest,
            PersonControllerTest personControllerTest,
            DepartmentControllerTest departmentControllerTest,
            AccountControllerTest accountControllerTest)
        {
            this.inventoryControllerTest = inventoryControllerTest;
            this.personControllerTest = personControllerTest;
            this.departmentControllerTest = departmentControllerTest;
            this.accountControllerTest = accountControllerTest;
        }

        public async Task<List<AAInventoryModel>> GetAAInventoriesAsync(bool byPassMediatR = true)
        {
            var inventories = await inventoryControllerTest.GetInventoriesAsync(byPassMediatR);

            if (inventories != null && !inventories.Any())
            {
                await CreateAAInventoryAsync(byPassMediatR);

                inventories = await inventoryControllerTest.GetInventoriesAsync(byPassMediatR);
            }

            return inventories;
        }

        public async Task<ServiceResultModel> CreateAAInventoryAsync(bool byPassMediatR = true)
        {
            return await inventoryControllerTest.CreateInventoryAsync(new AACreateInventoryCommandRequest()
            {
                Inventory = new AAInventoryModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            }, byPassMediatR);
        }

        public async Task<List<DepartmentModel>> GetAADepartmentsAsync(bool byPassMediatR = true)
        {
            var departments = await departmentControllerTest.GetDepartmentsAsync(byPassMediatR);

            if (departments != null && !departments.Any())
            {
                var createDepartmentResult = departmentControllerTest.CreateDepartmentAsync(new CreateDepartmentCommandRequest()
                {
                    Department = new DepartmentModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                }, byPassMediatR);

                departments = await departmentControllerTest.GetDepartmentsAsync(byPassMediatR);
            }

            return departments;
        }

        public async Task<List<WorkerModel>> GetWorkersAsync(bool byPassMediatR = true)
        {
            var workers = await personControllerTest.GetWorkersAsync(byPassMediatR);

            if (workers != null && !workers.Any())
            {
                var people = await personControllerTest.GetPeopleAsync(byPassMediatR);

                if (people != null && !people.Any())
                {
                    var createPeopleResult = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                    {
                        Person = new PersonModel()
                        {
                            Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                        }
                    }, byPassMediatR);

                    people = await personControllerTest.GetPeopleAsync(byPassMediatR);
                }

                var createWorkerResult = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
                {
                    Worker = new WorkerModel()
                    {
                        Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                    }
                }, byPassMediatR);

                workers = await personControllerTest.GetWorkersAsync(byPassMediatR);
            }

            return workers;
        }

        public async Task<ServiceResultModel> CreateBankAccountToWorker(int workerId, bool byPassMediatR = true)
        {
            return await accountControllerTest.CreateBankAccountTestAsync(new AccountingCreateBankAccountCommandRequest()
            {
                BankAccount = new AccountingBankAccountModel()
                {
                    IBAN = new Random().Next(int.MinValue, int.MaxValue).ToString(),
                    Worker = new AccountingWorkerModel()
                    {
                        Id = workerId,
                    }
                }
            }, byPassMediatR);
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentToWorker(int workerId, bool byPassMediatR = true)
        {
            var bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(workerId, byPassMediatR);

            if (bankAccounts != null && !bankAccounts.Any())
            {
                await CreateBankAccountToWorker(workerId, byPassMediatR);

                bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(workerId, byPassMediatR);
            }

            var randomBankAccount = bankAccounts.ElementAt(new Random().Next(0, bankAccounts.Count - 1));

            var currencies = await GetCurrenciesAsync(byPassMediatR);

            var randomCurrency = currencies.ElementAt(new Random().Next(0, currencies.Count - 1));

            var result = await accountControllerTest.CreateSalaryPaymentTest(new AccountingCreateSalaryPaymentCommandRequest()
            {
                SalaryPayment = new AccountingSalaryPaymentModel()
                {
                    Amount = new Random().Next(1, byte.MaxValue),
                    Date = DateTime.Now,
                    BankAccount = randomBankAccount,
                    Currency = randomCurrency
                }
            }, byPassMediatR);

            return result;
        }

        public async Task<List<AccountingCurrencyModel>> GetCurrenciesAsync(bool byPassMediatR = true)
        {
            var accounts = await accountControllerTest.GetCurrenciesAsync(byPassMediatR);

            if (accounts != null && !accounts.Any())
            {
                await accountControllerTest.CreateCurrencyAsync(new AccountingCreateCurrencyCommandRequest()
                {
                    Currency = new AccountingCurrencyModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                }, byPassMediatR);

                accounts = await accountControllerTest.GetCurrenciesAsync(byPassMediatR);
            }

            return accounts;
        }

        public async Task<List<PersonModel>> GetPeopleAsync(bool byPassMediatR = true)
        {
            var people = await personControllerTest.GetPeopleAsync(byPassMediatR);

            if (people != null && !people.Any())
            {
                var createPersonTask = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                {
                    Person = new PersonModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                }, byPassMediatR);

                people = await personControllerTest.GetPeopleAsync(byPassMediatR);
            }

            return people;
        }


        public async Task<List<TitleModel>> GetTitlesAsync(bool byPassMediatR = true)
        {
            var titles = await personControllerTest.GetTitles(byPassMediatR);

            if (titles != null && !titles.Any())
            {
                var createTitleTask = await personControllerTest.CreateTitle(new CreateTitleCommandRequest()
                {
                    Title = new TitleModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                }, byPassMediatR);

                titles = await personControllerTest.GetTitles(byPassMediatR);
            }

            return titles;
        }
    }
}
