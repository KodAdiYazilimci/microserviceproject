using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.IT.Controllers;
using Services.Communication.Http.Broker.Department.IT.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.IT.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.IT.Factories.Services;

namespace Test.Services.Api.Business.Departments.IT.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private InventoryController inventoryController = null;
        private RouteNameProvider routeNameProvider = null;
        private ServiceCommunicator serviceCommunicator = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            inventoryController = new InventoryController(InventoryServiceFactory.Instance);
            routeNameProvider = RouteNameProviderFactory.GetRouteNameProvider(ConfigurationFactory.GetConfiguration());

            serviceCommunicator =
                ServiceCommunicatorFactory.GetServiceCommunicator(
                    cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                    credentialProvider: CredentialProviderFactory.GetCredentialProvider(ConfigurationFactory.GetConfiguration()),
                    routeNameProvider: routeNameProvider,
                    serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(ConfigurationFactory.GetConfiguration()));
        }

        [TestMethod]
        public async Task GetInventoriesTest()
        {
            IActionResult inventoryResult = await inventoryController.GetInventories(cancellationTokenSource);

            Assert.IsInstanceOfType(inventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateInventoryTest()
        {
            IActionResult createInventoryResult = await inventoryController.CreateInventory(new InventoryModel()
            {
                Name = new Random().Next(int.MaxValue / 2, int.MaxValue).ToString()
            }, cancellationTokenSource);

            Assert.IsInstanceOfType(createInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {
            ServiceResultModel<List<WorkerModel>> workersResult =
                await serviceCommunicator.Call<List<WorkerModel>>(
                    serviceName: routeNameProvider.HR_GetWorkers,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult assignInventoryResult =
                await inventoryController.AssignInventoryToWorker(
                    worker: workersResult.Data.ElementAt(new Random().Next(0, workersResult.Data.Count - 1)),
                    cancellationTokenSource: cancellationTokenSource);

            Assert.IsInstanceOfType(assignInventoryResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            ServiceResultModel<List<InventoryModel>> inventoriesResult =
                await serviceCommunicator.Call<List<InventoryModel>>(
                    serviceName: routeNameProvider.IT_GetInventories,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult createResult =
                await inventoryController.CreateDefaultInventoryForNewWorker(
                    inventory: inventoriesResult.Data.ElementAt(new Random().Next(0, inventoriesResult.Data.Count - 1)),
                    cancellationTokenSource: cancellationTokenSource);

            Assert.IsInstanceOfType(createResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetInventoriesForNewWorkerTest()
        {
            IActionResult getInventoriesResult = inventoryController.GetInventoriesForNewWorker(cancellationTokenSource);

            Assert.IsInstanceOfType(getInventoriesResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task InformInventoryRequestTest()
        {
            ServiceResultModel<List<InventoryRequestModel>> inventoryRequestsResult =
                await serviceCommunicator.Call<List<InventoryRequestModel>>(
                    serviceName: routeNameProvider.Buying_GetInventoryRequests,
                    postData: null,
                    queryParameters: null,
                    headers: null,
                    cancellationTokenSource: cancellationTokenSource);

            IActionResult informInventoryResult =
                await inventoryController.InformInventoryRequest(
                    inventoryRequest: inventoryRequestsResult.Data.ElementAt(new Random().Next(0, inventoryRequestsResult.Data.Count - 1)),
                    cancellationTokenSource: cancellationTokenSource);

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
