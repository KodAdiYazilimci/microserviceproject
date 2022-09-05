
using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.HR;
using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.HR.Factories.Services;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class PersonControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private PersonController personController = null;
        private DepartmentController departmentController = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            personController = new PersonController(MediatorFactory.GetInstance(typeof(Startup)), PersonServiceFactory.Instance);
            departmentController = new DepartmentController(MediatorFactory.GetInstance(typeof(Startup)), DepartmentServiceFactory.Instance);
        }

        [TestMethod]
        public async Task GetPeopleTest()
        {
            IActionResult getPeopleResult = await personController.GetPeople();

            Assert.IsInstanceOfType(getPeopleResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreatePersonTest()
        {
            IActionResult createPersonResult = await personController.CreatePerson(
                new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreatePersonCommandRequest()
                {
                    Person = new PersonModel()
                    {
                        Name = new Random().Next(0, int.MaxValue).ToString()
                    }
                });

            Assert.IsInstanceOfType(createPersonResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetTitlesTest()
        {
            IActionResult getTitlesResult = await personController.GetTitles();

            Assert.IsInstanceOfType(getTitlesResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateTitleTest()
        {
            IActionResult createTitleResult = await personController.CreateTitle(
                new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreateTitleCommandRequest()
                {
                    Title = new TitleModel
                    {
                        Name = new Random().Next(0, int.MaxValue).ToString()
                    }
                });

            Assert.IsInstanceOfType(createTitleResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetWorkersTest()
        {
            IActionResult getWorkersResult = await personController.GetWorkers();

            Assert.IsInstanceOfType(getWorkersResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var titleTask = personController.GetTitles();
            var peopleTask = personController.GetPeople();
            var departmentTask = departmentController.GetDepartments();

            Task.WaitAll(titleTask, peopleTask, departmentTask);

            ServiceResultModel<List<TitleModel>> titles = (titleTask.Result as OkObjectResult).Value as ServiceResultModel<List<TitleModel>>;
            ServiceResultModel<List<PersonModel>> people = (peopleTask.Result as OkObjectResult).Value as ServiceResultModel<List<PersonModel>>;
            ServiceResultModel<List<DepartmentModel>> departments = (departmentTask.Result as OkObjectResult).Value as ServiceResultModel<List<DepartmentModel>>;

            IActionResult CreateWorkerResult = await personController.CreateWorker(
                new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreateWorkerCommandRequest()
                {
                    Worker = new WorkerModel
                    {
                        Person = people.Data.ElementAt(new Random().Next(0, people.Data.Count - 1)),
                        Title = titles.Data.ElementAt(new Random().Next(0, titles.Data.Count - 1)),
                        Department = departments.Data.ElementAt(new Random().Next(0, departments.Data.Count - 1)),
                        FromDate = DateTime.UtcNow,
                        BankAccounts = new List<BankAccountModel>()
                        {
                            new BankAccountModel(){ IBAN = new Random().Next(int.MaxValue/2,int.MaxValue).ToString() }
                        }
                    },
                });

            Assert.IsInstanceOfType(CreateWorkerResult, typeof(OkObjectResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            cancellationTokenSource = null;
            personController.Dispose();
            personController = null;
            departmentController.Dispose();
            departmentController = null;
        }
    }
}
