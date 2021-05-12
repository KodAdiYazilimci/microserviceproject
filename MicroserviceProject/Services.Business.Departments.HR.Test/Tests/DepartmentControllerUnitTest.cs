using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Services.Business.Departments.HR.Controllers;
using MicroserviceProject.Services.Business.Departments.HR.Test.Factories.Services;
using MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure;
using MicroserviceProject.Test.Services.Factories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Tests
{
    [TestClass]
    public class DepartmentControllerUnitTest
    {
        private CancellationTokenSource cancellationTokenSource = null;
        private DepartmentController departmentController = null;
        private RouteNameProvider routeNameProvider = null;
        private ServiceCommunicator serviceCommunicator = null;

        [TestInitialize]
        public void Init()
        {
            cancellationTokenSource = new CancellationTokenSource();
            departmentController = new DepartmentController(DepartmentServiceFactory.Instance);
            routeNameProvider = RouteNameProviderFactory.GetRouteNameProvider(ConfigurationFactory.GetConfiguration());

            serviceCommunicator =
                ServiceCommunicatorFactory.GetServiceCommunicator(
                    memoryCache: MemoryCacheFactory.Instance,
                    credentialProvider: CredentialProviderFactory.GetCredentialProvider(ConfigurationFactory.GetConfiguration()),
                    routeNameProvider: routeNameProvider,
                    serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(ConfigurationFactory.GetConfiguration()));
        }

        [TestMethod]
        public async Task GetDepartmentsTest()
        {
            IActionResult departmentResult = await departmentController.GetDepartments(cancellationTokenSource);

            Assert.IsInstanceOfType(departmentResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetDepartmentsWithServiceCommunicatorTest()
        {
            ServiceResultModel<List<DepartmentModel>> serviceResult =
                await
                serviceCommunicator.Call<List<DepartmentModel>>(
                serviceName: routeNameProvider.HR_GetDepartments,
                postData: null,
                queryParameters: null,
                cancellationTokenSource: cancellationTokenSource);

            Assert.IsTrue(serviceResult.IsSuccess);
        }


        [TestMethod]
        public async Task CreateDepartmentTask()
        {
            IActionResult createDepartmentResult = await departmentController.CreateDepartment(new DepartmentModel()
            {
                Name = new Random().Next(0, int.MaxValue).ToString()
            }, cancellationTokenSource);

            Assert.IsInstanceOfType(createDepartmentResult, typeof(OkObjectResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            cancellationTokenSource = null;

            departmentController.Dispose();
            departmentController = null;

            serviceCommunicator.Dispose();
            serviceCommunicator = null;

            routeNameProvider.Dispose();
            routeNameProvider = null;
        }
    }
}
