using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Security.Authentication.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.AA;
using Services.Api.Business.Departments.AA.Controllers;
using Services.Communication.Http.Broker.Department.AA.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.AA.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.AA.Factories.Services;

namespace Test.Services.Api.Business.Departments.AA.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private InventoryController inventoryController = null;
        private ServiceCommunicator serviceCommunicator = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            inventoryController = new InventoryController(MediatorFactory.GetInstance(typeof(Startup)), InventoryServiceFactory.Instance);

            serviceCommunicator =
                ServiceCommunicatorFactory.GetServiceCommunicator(
                    cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                    credentialProvider: CredentialProviderFactory.GetCredentialProvider(ConfigurationFactory.GetConfiguration()),
                    serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(ConfigurationFactory.GetConfiguration()));
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
            ServiceResultModel<List<WorkerModel>> workersResult =
                await serviceCommunicator.Call<List<WorkerModel>>(
                    serviceName: "hr.person.getworkers",
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult assignInventoryResult =
                await inventoryController.AssignInventoryToWorker(
                    new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.AssignInventoryToWorkerCommandRequest()
                    {
                        Worker = workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1))
                    });

            Assert.IsInstanceOfType(assignInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            ServiceResultModel<List<InventoryModel>> inventoriesResult =
                await serviceCommunicator.Call<List<InventoryModel>>(
                    serviceName: "aa.inventory.getinventories",
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult createResult =
                await inventoryController.CreateDefaultInventoryForNewWorker(
                    new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.CreateDefaultInventoryForNewWorkerCommandRequest()
                    {
                        Inventory = inventoriesResult.Data.ElementAt(new Random().Next(0, inventoriesResult.Data.Count - 1))
                    });

            Assert.IsInstanceOfType(createResult, typeof(OkObjectResult));
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
            ServiceResultModel<List<InventoryRequestModel>> inventoryRequestsResult =
                await serviceCommunicator.Call<List<InventoryRequestModel>>(
                    serviceName: "buying.request.getinventoryrequests",
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult informInventoryResult =
                await inventoryController.InformInventoryRequest(
                    new global::Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests.InformInventoryRequestCommandRequest()
                    {
                        InventoryRequest = inventoryRequestsResult.Data.ElementAt(new Random().Next(0, inventoryRequestsResult.Data.Count - 1))
                    });

            Assert.IsInstanceOfType(informInventoryResult, typeof(OkObjectResult));
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
