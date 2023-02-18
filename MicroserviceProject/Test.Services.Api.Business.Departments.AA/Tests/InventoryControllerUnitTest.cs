using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.HR;

namespace Test.Services.Api.Business.Departments.AA.Tests
{
    [TestClass]
    public class InventoryControllerUnitTest
    {
        private DepartmentControllerTest departmentControllerTest;
        private PersonControllerTest personControllerTest;
        private InventoryControllerTest inventoryControllerTest;

        [TestInitialize]
        public void Init()
        {
            departmentControllerTest = new DepartmentControllerTest();
            inventoryControllerTest = new InventoryControllerTest();
            personControllerTest = new PersonControllerTest();
        }

        [TestMethod]
        public async Task GetInventoriesTest()
        {
            List<global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel> inventories = await GetInventoriesAsync();

            Assert.IsTrue(inventories != null && inventories.Any());
        }

        private async Task<List<global::Services.Communication.Http.Broker.Department.AA.Models.InventoryModel>> GetInventoriesAsync()
        {
            var inventories = await inventoryControllerTest.GetInventoriesAsync();

            if (inventories != null && !inventories.Any())
            {
                await CreateInventoryTest();

                inventories = await inventoryControllerTest.GetInventoriesAsync();
            }

            return inventories;
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
            List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel> workers = await GetWorkersAsync();

            var inventories = await GetInventoriesAsync();

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

        private async Task<List<global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel>> GetWorkersAsync()
        {
            var workers = await personControllerTest.GetWorkersAsync();

            if (workers != null && !workers.Any())
            {
                List<PersonModel> people = await GetPeopleAsync();

                var createWorkerResult = await personControllerTest.CreateWorkerAsync(new CreateWorkerCommandRequest()
                {
                    Worker = new global::Services.Communication.Http.Broker.Department.HR.Models.WorkerModel()
                    {
                        Person = people.ElementAt(new Random().Next(0, people.Count - 1))
                    }
                });

                workers = await personControllerTest.GetWorkersAsync();
            }

            return workers;
        }

        private async Task<List<PersonModel>> GetPeopleAsync()
        {
            var people = await personControllerTest.GetPeopleAsync();

            if (people != null && !people.Any())
            {
                var createPersonTask = personControllerTest.CreatePersonAsync(new CreatePersonCommandRequest()
                {
                    Person = new PersonModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                people = await personControllerTest.GetPeopleAsync();
            }

            return people;
        }

        [TestMethod]
        public async Task CreateDefaultInventoryForNewWorkerTest()
        {
            var inventories = await GetInventoriesAsync();

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
            var inventories = await GetInventoriesAsync();

            List<DepartmentModel> departments = await GetDepartmentsAsync();

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

        private async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            var departments = await departmentControllerTest.GetDepartmentsAsync();

            if (departments != null && !departments.Any())
            {
                var createDepartmentResult = departmentControllerTest.CreateDepartmentAsync(new CreateDepartmentCommandRequest()
                {
                    Department = new DepartmentModel()
                    {
                        Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                    }
                });

                departments = await departmentControllerTest.GetDepartmentsAsync();
            }

            return departments;
        }

        [TestCleanup]
        public void CleanUp()
        {
            inventoryControllerTest = null;
            personControllerTest = null;
        }
    }
}
