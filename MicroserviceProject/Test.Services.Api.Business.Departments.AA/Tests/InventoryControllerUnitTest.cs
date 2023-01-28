using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.HR.Controllers;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.AA.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private global::Services.Api.Business.Departments.AA.Controllers.InventoryController inventoryController = null;
        private PersonController personController = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            inventoryController = new global::Services.Api.Business.Departments.AA.Controllers.InventoryController(
                MediatorFactory.GetInstance(typeof(global::Services.Api.Business.Departments.AA.Startup)),
                global::Test.Services.Api.Business.Departments.AA.Factories.Services.InventoryServiceFactory.Instance);
            personController = new PersonController(
                MediatorFactory.GetInstance(typeof(global::Services.Api.Business.Departments.HR.Startup)),
                global::Test.Services.Api.Business.Departments.HR.Factories.Services.PersonServiceFactory.Instance);
        }

        [TestMethod]
        public async Task GetInventoriesTest()
        {
            IActionResult inventoryResult = await inventoryController.GetInventories();

            Assert.IsInstanceOfType(inventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateInventoryTest()
        {
            IActionResult createInventoryResult = await inventoryController.CreateInventory(
                new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.CreateInventoryCommandRequest()
                {
                    Inventory = new global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel()
                    {
                        Name = new Random().Next(int.MaxValue / 2, int.MaxValue).ToString()
                    }
                });

            Assert.IsInstanceOfType(createInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {
            //IActionResult workersResult =
            //    await personController.GetWorkers();

            //IActionResult assignInventoryResult =
            //    await inventoryController.AssignInventoryToWorker(
            //        new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.AssignInventoryToWorkerCommandRequest()
            //        {
            //            Worker = workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1))
            //        });

            //Assert.IsInstanceOfType(assignInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            //IActionResult createResult =
            //    await inventoryController.CreateDefaultInventoryForNewWorker(
            //        new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.CreateDefaultInventoryForNewWorkerCommandRequest()
            //        {
            //            Inventory = inventoriesResult.Data.ElementAt(new Random().Next(0, inventoriesResult.Data.Count - 1))
            //        });

            //Assert.IsInstanceOfType(createResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetInventoriesForNewWorkerTest()
        {
            IActionResult getInventoriesResult = inventoryController.GetInventoriesForNewWorker();

            Assert.IsInstanceOfType(getInventoriesResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task InformInventoryRequestTest()
        {
            //IActionResult informInventoryResult =
            //    await inventoryController.InformInventoryRequest(
            //        new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.InformInventoryRequestCommandRequest()
            //        {
            //            InventoryRequest = inventoryRequestsResult.Data.ElementAt(new Random().Next(0, inventoryRequestsResult.Data.Count - 1))
            //        });

            //Assert.IsInstanceOfType(informInventoryResult, typeof(OkObjectResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            cancellationTokenSource = null;
            inventoryController.Dispose();
            inventoryController = null;
        }
    }
}
