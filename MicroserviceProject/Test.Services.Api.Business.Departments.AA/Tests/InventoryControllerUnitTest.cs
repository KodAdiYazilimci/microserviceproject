using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Authorization.Mock;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Mock;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Broker.Department.HR.Models;
using Services.Util.Exception.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.AA.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private InventoryControllerTest inventoryControllerTest;

        [TestInitialize]
        public void Init()
        {
            inventoryControllerTest = new InventoryControllerTest();
        }

        [TestMethod]
        public async Task GetInventoriesTest()
        {
            var inventories = await inventoryControllerTest.GetInventoriesAsync();

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        [TestMethod]
        public async Task CreateInventoryTest()
        {
            var result = await inventoryControllerTest.CreateInventoryAsync(new CreateInventoryCommandRequest()
            {
                Inventory = new global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel()
                {
                    CurrentStockCount = 0,
                    FromDate = DateTime.Now,
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString(),
                    ToDate = DateTime.Now.AddDays(new Random().Next(1, byte.MaxValue))
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestMethod]
        public async Task AssignInventoryToWorkerTest()
        {

        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {

        }

        [TestMethod]
        public void GetInventoriesForNewWorkerTest()
        {

        }

        [TestMethod]
        public async Task InformInventoryRequestTest()
        {

        }

        [TestCleanup]
        public void CleanUp()
        {

        }
    }
}
