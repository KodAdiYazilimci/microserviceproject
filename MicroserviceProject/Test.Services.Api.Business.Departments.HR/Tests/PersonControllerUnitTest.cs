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
    public class PersonControllerUnitTest : BaseTest
    {
        private DepartmentControllerTest departmentControllerTest = new DepartmentControllerTest();
        private PersonControllerTest personControllerTest = new PersonControllerTest();

        public PersonControllerUnitTest(InventoryControllerTest inventoryControllerTest, PersonControllerTest personControllerTest, DepartmentControllerTest departmentControllerTest, AccountControllerTest accountControllerTest) : base(inventoryControllerTest, personControllerTest, departmentControllerTest, accountControllerTest)
        {
        }

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public async Task GetPeopleTest()
        {
            List<PersonModel> people = await GetPeopleAsync();

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
            var workers = await GetWorkersAsync();

            Assert.IsTrue(workers != null && workers.Any());
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var departments = await GetAADepartmentsAsync();

            var people = await GetPeopleAsync();

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
