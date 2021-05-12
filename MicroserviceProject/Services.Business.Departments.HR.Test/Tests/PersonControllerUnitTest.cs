using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Department.Accounting;
using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;
using MicroserviceProject.Services.Business.Departments.HR.Controllers;
using MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Tests
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
            personController = new PersonController(PersonServiceFactory.Instance);
            departmentController = new DepartmentController(DepartmentServiceFactory.Instance);
        }

        [TestMethod]
        public async Task GetPeopleTest()
        {
            IActionResult getPeopleResult = await personController.GetPeople(cancellationTokenSource);

            Assert.IsInstanceOfType(getPeopleResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreatePersonTest()
        {
            IActionResult createPersonResult = await personController.CreatePerson(new PersonModel()
            {
                Name = new Random().Next(0, int.MaxValue).ToString()
            }, cancellationTokenSource);

            Assert.IsInstanceOfType(createPersonResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetTitlesTest()
        {
            IActionResult getTitlesResult = await personController.GetTitles(cancellationTokenSource);

            Assert.IsInstanceOfType(getTitlesResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateTitleTest()
        {
            IActionResult createTitleResult = await personController.CreateTitle(new TitleModel
            {
                Name = new Random().Next(0, int.MaxValue).ToString()
            }, cancellationTokenSource);

            Assert.IsInstanceOfType(createTitleResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetWorkersTest()
        {
            IActionResult getWorkersResult = await personController.GetWorkers(cancellationTokenSource);

            Assert.IsInstanceOfType(getWorkersResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateWorkerTest()
        {
            var titleTask = personController.GetTitles(cancellationTokenSource);
            var peopleTask = personController.GetPeople(cancellationTokenSource);
            var departmentTask = departmentController.GetDepartments(cancellationTokenSource);

            Task.WaitAll(titleTask, peopleTask, departmentTask);

            ServiceResultModel<List<TitleModel>> titles = (titleTask.Result as OkObjectResult).Value as ServiceResultModel<List<TitleModel>>;
            ServiceResultModel<List<PersonModel>> people = (peopleTask.Result as OkObjectResult).Value as ServiceResultModel<List<PersonModel>>;
            ServiceResultModel<List<DepartmentModel>> departments = (departmentTask.Result as OkObjectResult).Value as ServiceResultModel<List<DepartmentModel>>;

            IActionResult CreateWorkerResult = await personController.CreateWorker(new WorkerModel
            {
                Person = people.Data.ElementAt(new Random().Next(0, people.Data.Count - 1)),
                Title = titles.Data.ElementAt(new Random().Next(0, titles.Data.Count - 1)),
                Department = departments.Data.ElementAt(new Random().Next(0, departments.Data.Count - 1)),
                FromDate = DateTime.Now,
                BankAccounts = new List<BankAccountModel>()
                {
                    new BankAccountModel(){ IBAN = new Random().Next(int.MaxValue/2,int.MaxValue).ToString() }
                }
            }, cancellationTokenSource);

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
