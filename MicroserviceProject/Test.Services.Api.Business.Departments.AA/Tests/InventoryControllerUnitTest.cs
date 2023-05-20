using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.Accounting;
using Test.Services.Api.Business.Departments.HR;

namespace Test.Services.Api.Business.Departments.AA.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
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
        public async Task GetInventoriesTest()
        {
            List<AAInventoryModel> inventories = await dataProvider.GetAAInventoriesAsync(byPassMediatR: true);

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        [TestMethod]
        public async Task CreateInventoryTest()
        {
            Infrastructure.Communication.Http.Models.ServiceResultModel result = await dataProvider.CreateAAInventoryAsync(byPassMediatR: true);

            Assert.IsTrue(result != null && result.IsSuccess);
        }


        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {
            List<WorkerModel> workers = await dataProvider.GetWorkersAsync();

            var inventories = await dataProvider.GetAAInventoriesAsync(byPassMediatR: true);

            var result = await inventoryControllerTest.AssignInventoryToWorkerTest(new AAAssignInventoryToWorkerCommandRequest()
            {
                AssignInventoryToWorkerModels = new List<AAAssignInventoryToWorkerModel>()
                {
                    new AAAssignInventoryToWorkerModel()
                    {
                        WorkerId = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id,
                        InventoryId = inventories.ElementAt(new Random().Next(0, inventories.Count-1)).Id,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddDays(new Random().Next(1, byte.MaxValue)),
                        Amount = new Random().Next(1, byte.MaxValue)
                    }
                }
            }, byPassMediatR: true);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            await CreateDefaultInventoryForNewWorkerTest(byPassMediatR: true);
        }

        private async Task CreateDefaultInventoryForNewWorkerTest(bool byPassMediatR = true)
        {
            var inventories = await dataProvider.GetAAInventoriesAsync(byPassMediatR);

            var result = await inventoryControllerTest.CreateDefaultInventoryForNewWorker(new AACreateDefaultInventoryForNewWorkerCommandRequest()
            {
                DefaultInventoryForNewWorkerModel = new AADefaultInventoryForNewWorkerModel()
                {
                    Id = inventories.ElementAt(new Random().Next(0, inventories.Count - 1)).Id,
                    Amount = new Random().Next(1, byte.MaxValue)
                }
            }, byPassMediatR);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task GetInventoriesForNewWorkerTest()
        {
            var inventories = await inventoryControllerTest.GetInventoriesForNewWorker(byPassMediatR: true);

            if (inventories != null && !inventories.Any())
            {
                await CreateDefaultInventoryForNewWorkerTest(byPassMediatR: true);

                inventories = await inventoryControllerTest.GetInventoriesForNewWorker(byPassMediatR: true);
            }

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        [TestMethod]
        public async Task InformInventoryRequestTest()
        {
            var inventories = await dataProvider.GetAAInventoriesAsync(byPassMediatR: true);

            List<DepartmentModel> departments = await dataProvider.GetAADepartmentsAsync(byPassMediatR: true);

            var result = await inventoryControllerTest.InformInventoryRequest(new AAInformInventoryRequestCommandRequest()
            {
                InventoryRequest = new AAInventoryRequestModel()
                {
                    Amount = new Random().Next(1, byte.MaxValue),
                    InventoryId = inventories.ElementAt(new Random().Next(0, inventories.Count - 1)).Id
                }
            }, byPassMediatR: true);

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestCleanup]
        public void CleanUp()
        {
            inventoryControllerTest = null;
            personControllerTest = null;
        }
    }
}
