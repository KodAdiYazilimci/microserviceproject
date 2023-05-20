using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.AA;
using Test.Services.Api.Business.Departments.Accounting;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class PersonControllerUnitTest
    {
        private AccountControllerTest accountControllerTest;
        private DepartmentControllerTest departmentControllerTest;
        private PersonControllerTest personControllerTest;
        private InventoryControllerTest inventoryControllerTest;

        private DataProvider dataProvider;

        [TestInitialize]
        public void Init()
        {
            accountControllerTest = new AccountControllerTest();
            departmentControllerTest = new DepartmentControllerTest();
            personControllerTest = new PersonControllerTest();
            inventoryControllerTest = new InventoryControllerTest();
            dataProvider = new DataProvider(inventoryControllerTest, personControllerTest, departmentControllerTest, accountControllerTest);
        }

        [TestMethod]
        public async Task GetPeopleTest()
        {
            List<PersonModel> people = await dataProvider.GetPeopleAsync(byPassMediatR: true);

            Assert.IsNotNull(people != null && people.Any());
        }

        [TestMethod]
        public async Task CreatePersonTest()
        {
            var result = await personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
            {
                Person = new PersonModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            }, byPassMediatR: true);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetTitlesTest()
        {
            var titles = await personControllerTest.GetTitles(byPassMediatR: true);

            if (titles != null && !titles.Any())
            {
                await CreateTitleTest(byPassMediatR: true);

                titles = await personControllerTest.GetTitles(byPassMediatR: true);
            }

            Assert.IsTrue(titles != null && titles.Any());
        }

        [TestMethod]
        public async Task CreateTitleTest()
        {
            await CreateTitleTest(byPassMediatR: true);
        }

        public async Task CreateTitleTest(bool byPassMediatR = true)
        {
            var result = await personControllerTest.CreateTitle(new CreateTitleCommandRequest()
            {
                Title = new TitleModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            }, byPassMediatR);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWorkersTest()
        {
            var workers = await dataProvider.GetWorkersAsync(byPassMediatR: true);

            Assert.IsTrue(workers != null && workers.Any());
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var departments = await dataProvider.GetAADepartmentsAsync(byPassMediatR: true);

            var people = await dataProvider.GetPeopleAsync(byPassMediatR: true);

            var titles = await dataProvider.GetTitlesAsync(byPassMediatR: true);

            var result = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
            {
                Worker = new WorkerModel()
                {
                    Department = departments.ElementAt(new Random().Next(0, departments.Count - 1)),
                    Person = people.ElementAt(new Random().Next(0, people.Count - 1)),
                    Title = titles.ElementAt(new Random().Next(0, titles.Count - 1)),
                    BankAccounts = new List<BankAccountModel>()
                    {
                        new BankAccountModel()
                        {
                             IBAN = new Random().Next(int.MinValue, int.MaxValue).ToString()
                        }
                    },
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(new Random().Next(byte.MinValue, byte.MaxValue))
                }
            }, byPassMediatR: true);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }
    }
}
