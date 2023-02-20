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
            List<PersonModel> people = await dataProvider.GetPeopleAsync();

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
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetTitlesTest()
        {
            var titles = await personControllerTest.GetTitles();

            if (titles != null && !titles.Any())
            {
                await CreateTitleTest();

                titles = await personControllerTest.GetTitles();
            }

            Assert.IsTrue(titles != null && titles.Any());
        }

        [TestMethod]
        public async Task CreateTitleTest()
        {
            var result = await personControllerTest.CreateTitle(new CreateTitleCommandRequest()
            {
                Title = new TitleModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWorkersTest()
        {
            var workers = await dataProvider.GetWorkersAsync();

            Assert.IsTrue(workers != null && workers.Any());
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var departments = await dataProvider.GetAADepartmentsAsync();

            var people = await dataProvider.GetPeopleAsync();

            var result = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
            {
                Worker = new WorkerModel()
                {
                    Department = departments.ElementAt(new Random().Next(0, departments.Count - 1)),
                    Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }
    }
}
