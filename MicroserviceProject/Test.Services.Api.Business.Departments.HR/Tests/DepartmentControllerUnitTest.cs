using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.AA;
using Test.Services.Api.Business.Departments.Accounting;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
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
        public async Task GetDepartmentsTest()
        {
            var departments = await departmentControllerTest.GetDepartmentsAsync();

            if (departments != null && !departments.Any())
            {
                await CreateDepartmentTask();

                departments = await departmentControllerTest.GetDepartmentsAsync();
            }

            Assert.IsTrue(departments != null && departments.Any());
        }

        [TestMethod]
        public async Task CreateDepartmentTask()
        {
            var result = await departmentControllerTest.CreateDepartmentAsync(new CreateDepartmentCommandRequest()
            {
                Department = new DepartmentModel()
                {
                    Name = new Random().Next(int.MinValue, int.MaxValue).ToString()
                }
            });

            Assert.IsTrue(result != null && result.IsSuccess);
        }

        [TestCleanup]
        public void CleanUp()
        {
            departmentControllerTest = null;
        }
    }
}
