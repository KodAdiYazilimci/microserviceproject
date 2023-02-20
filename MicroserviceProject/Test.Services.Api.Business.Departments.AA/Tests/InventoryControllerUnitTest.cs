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
            accountControllerTest= new AccountControllerTest();
            departmentControllerTest = new DepartmentControllerTest();
            personControllerTest = new PersonControllerTest();
            inventoryControllerTest = new InventoryControllerTest();
            dataProvider = new DataProvider(inventoryControllerTest, personControllerTest, departmentControllerTest, accountControllerTest);
        }

        [TestMethod]
        public async Task GetInventoriesTest()
        {
            List<global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel> inventories = await dataProvider.GetAAInventoriesAsync();

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        [TestMethod]
        public async Task CreateInventoryTest()
        {
            Infrastructure.Communication.Http.Models.ServiceResultModel result = await dataProvider.CreateAAInventoryAsync();

            Assert.IsTrue(result != null && result.IsSuccess);
        }


        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {
            List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel> workers = await dataProvider.GetWorkersAsync();

            var inventories = await dataProvider.GetAAInventoriesAsync();

            var result = await inventoryControllerTest.AssignInventoryToWorkerTest(new AssignInventoryToWorkerCommandRequest()
            {
                Worker = new global::Services.Communication.Http.Broker.Department.AA.Models.WorkerModel()
                {
                    Id = workers.ElementAt(new Random().Next(0, workers.Count - 1)).Id,
                    AAInventories = inventories.Take(new Random().Next(1, inventories.Count)).ToList(),
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(new Random().Next(1, byte.MaxValue))
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            var inventories = await dataProvider.GetAAInventoriesAsync();

            var result = await inventoryControllerTest.CreateDefaultInventoryForNewWorker(new CreateDefaultInventoryForNewWorkerCommandRequest()
            {
                Inventory = new global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel()
                {
                    CurrentStockCount = new Random().Next(1, byte.MaxValue),
                    FromDate = DateTime.Now,
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString(),
                    ToDate = DateTime.Now.AddDays(new Random().Next(1, byte.MaxValue)),
                    Id = inventories.ElementAt(new Random().Next(0, inventories.Count - 1)).Id
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async void GetInventoriesForNewWorkerTest()
        {
            var inventories = inventoryControllerTest.GetInventoriesForNewWorker();

            if (inventories != null && !inventories.Any())
            {
                await CreateDefaultInventoryForNewWorkerTest();

                inventories = inventoryControllerTest.GetInventoriesForNewWorker();
            }

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        [TestMethod]
        public async Task InformInventoryRequestTest()
        {
            var inventories = await dataProvider.GetAAInventoriesAsync();

            List<DepartmentModel> departments = await dataProvider.GetAADepartmentsAsync();

            var result = await inventoryControllerTest.InformInventoryRequest(new InformInventoryRequestCommandRequest()
            {
                InventoryRequest = new InventoryRequestModel()
                {
                    Amount = new Random().Next(1, byte.MaxValue),
                    InventoryId = inventories.ElementAt(new Random().Next(0, inventories.Count - 1)).Id,
                    DepartmentId = departments.ElementAt(new Random().Next(0, departments.Count - 1)).Id
                }
            });

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
