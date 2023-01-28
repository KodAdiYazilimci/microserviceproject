using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.HR;
using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Services.Api.Business.Departments.HR.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private DepartmentController departmentController = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            //departmentController = new DepartmentController(MediatorFactory.GetInstance(typeof(Startup)), DepartmentServiceFactory.Instance, null);
        }

        [TestMethod]
        public async Task GetDepartmentsTest()
        {
            IActionResult departmentResult = await departmentController.GetDepartments();

            Assert.IsInstanceOfType(departmentResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDepartmentsWithServiceCommunicatorTest()
        {
            //ServiceResultModel<List<DepartmentModel>> serviceResult =
            //    await
            //    serviceCommunicator.Call<List<DepartmentModel>>(
            //    serviceName: "hr.department.getdepartments",
            //    postData: null,
            //    queryParameters: null,
            //    headers: null,
            //    cancellationTokenSource: cancellationTokenSource);

            //Assert.IsTrue(serviceResult.IsSuccess);
        }


        [TestMethod]
        public async Task CreateDepartmentTask()
        {
            IActionResult createDepartmentResult = await departmentController.CreateDepartment(
                new global::Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests.CreateDepartmentCommandRequest()
                {
                    Department = new DepartmentModel()
                    {
                        Name = new Random().Next(0, int.MaxValue).ToString()
                    }
                });

            Assert.IsInstanceOfType(createDepartmentResult, typeof(OkObjectResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            cancellationTokenSource = null;

            departmentController.Dispose();
            departmentController = null;
        }
    }
}
