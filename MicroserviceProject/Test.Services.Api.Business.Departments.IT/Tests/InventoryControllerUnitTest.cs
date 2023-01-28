using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.IT;
using Services.Api.Business.Departments.IT.Controllers;
using Services.Communication.Http.Broker.Department.IT.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.IT.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private InventoryController inventoryController = null;

        [TestInitialize]
        public void Init()
        {
            //cancellationTokenSource = new CancellationTokenSource();
            //inventoryController = new InventoryController(MediatorFactory.GetInstance(typeof(Startup)), InventoryServiceFactory.Instance);
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
                new global::Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests.CreateInventoryCommandRequest()
                {
                    Inventory = new InventoryModel()
                    {
                        Name = new Random().Next(int.MaxValue / 2, int.MaxValue).ToString()
                    }
                });

            Assert.IsInstanceOfType(createInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {
            //ServiceResultModel<List<WorkerModel>> workersResult =
            //    await serviceCommunicator.Call<List<WorkerModel>>(
            //        serviceName: "hr.person.getworkers",
            //        postData: null,
            //        queryParameters: null,
            //        headers: null,
            //        cancellationTokenSource: cancellationTokenSource);

            //IActionResult assignInventoryResult =
            //    await inventoryController.AssignInventoryToWorker(
            //        request: new global::Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests.AssignInventoryToWorkerCommandRequest()
            //        {
            //            Worker = workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1))
            //        });

            //Assert.IsInstanceOfType(assignInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            //ServiceResultModel<List<InventoryModel>> inventoriesResult =
            //    await serviceCommunicator.Call<List<InventoryModel>>(
            //        serviceName: "it.inventory.getinventories",
            //        postData: null,
            //        queryParameters: null,
            //        headers: null,
            //        cancellationTokenSource: cancellationTokenSource);

            //IActionResult createResult =
            //    await inventoryController.CreateDefaultInventoryForNewWorker(
            //        request: new global::Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests.CreateDefaultInventoryForNewWorkerCommandRequest()
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
            //ServiceResultModel<List<InventoryRequestModel>> inventoryRequestsResult =
            //    await serviceCommunicator.Call<List<InventoryRequestModel>>(
            //        serviceName: "buying.request.getinventoryrequests",
            //        postData: null,
            //        queryParameters: null,
            //        headers: null,
            //        cancellationTokenSource: cancellationTokenSource);

            //IActionResult informInventoryResult =
            //    await inventoryController.InformInventoryRequest(
            //        request: new global::Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests.InformInventoryRequestCommandRequest()
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
