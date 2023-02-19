using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
    {
        private DepartmentControllerTest departmentControllerTest;

        [TestInitialize]
        public void Init()
        {
            departmentControllerTest = new DepartmentControllerTest();
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
