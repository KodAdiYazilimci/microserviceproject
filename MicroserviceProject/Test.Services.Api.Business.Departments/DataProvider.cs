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

        public async Task<List<AAInventoryModel>> GetAAInventoriesAsync()
        {
            var inventories = await inventoryControllerTest.GetInventoriesAsync();

            if (inventories != null && !inventories.Any())
            {
                await CreateAAInventoryAsync();

                inventories = await inventoryControllerTest.GetInventoriesAsync();
            }

            return inventories;
        }

        public async Task<ServiceResultModel> CreateAAInventoryAsync()
        {
            return await inventoryControllerTest.CreateInventoryAsync(new AACreateInventoryCommandRequest()
            {
                Inventory = new AAInventoryModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });
        }

        public async Task<List<DepartmentModel>> GetAADepartmentsAsync()
        {
            var departments = await departmentControllerTest.GetDepartmentsAsync();

            if (departments != null && !departments.Any())
            {
                var createDepartmentResult = departmentControllerTest.CreateDepartmentAsync(new CreateDepartmentCommandRequest()
                {
                    Department = new DepartmentModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                departments = await departmentControllerTest.GetDepartmentsAsync();
            }

            return departments;
        }

        public async Task<List<WorkerModel>> GetWorkersAsync()
        {
            var workers = await personControllerTest.GetWorkersAsync();

            if (workers != null && !workers.Any())
            {
                var people = await personControllerTest.GetPeopleAsync();

                if (people != null && !people.Any())
                {
                    var createPeopleResult = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                    {
                        Person = new PersonModel()
                        {
                            Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                        }
                    });

                    people = await personControllerTest.GetPeopleAsync();
                }

                var createWorkerResult = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
                {
                    Worker = new WorkerModel()
                    {
                        Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                    }
                });

                workers = await personControllerTest.GetWorkersAsync();
            }

            return workers;
        }

        public async Task<ServiceResultModel> CreateBankAccountToWorker(int workerId)
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
            });
        }

        public async Task<ServiceResultModel> CreateSalaryPaymentToWorker(int workerId)
        {
            var bankAccounts = await accountControllerTest.GetBankAccountsOfWorkerAsync(workerId);

            var randomBankAccount = bankAccounts.ElementAt(new Random().Next(0, bankAccounts.Count - 1));

            var result = await accountControllerTest.CreateSalaryPaymentTest(new AccountingCreateSalaryPaymentCommandRequest()
            {
                SalaryPayment = new AccountingSalaryPaymentModel()
                {
                    Amount = new Random().Next(1, byte.MaxValue),
                    Date = DateTime.Now,
                    BankAccount = randomBankAccount
                }
            });
            return result;
        }

        public async Task<List<PersonModel>> GetPeopleAsync()
        {
            var people = await personControllerTest.GetPeopleAsync();

            if (people != null && !people.Any())
            {
                var createPersonTask = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                {
                    Person = new PersonModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                people = await personControllerTest.GetPeopleAsync();
            }

            return people;
        }


        public async Task<List<TitleModel>> GetTitlesAsync()
        {
            var titles = await personControllerTest.GetTitles();

            if (titles != null && !titles.Any())
            {
                var createTitleTask = await personControllerTest.CreateTitle(new CreateTitleCommandRequest()
                {
                    Title = new TitleModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                titles = await personControllerTest.GetTitles();
            }

            return titles;
        }
    }
}
