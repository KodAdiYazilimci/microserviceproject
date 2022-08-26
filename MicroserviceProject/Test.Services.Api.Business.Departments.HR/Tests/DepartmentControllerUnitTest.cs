using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Mock.Factories;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Services.Api.Business.Departments.HR;
using Services.Api.Business.Departments.HR.Controllers;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Test.Services.Api.Business.Departments.HR.Factories.Infrastructure;
using Test.Services.Api.Business.Departments.HR.Factories.Services;

namespace Test.Services.Api.Business.Departments.HR.Tests
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
            departmentController = new DepartmentController(MediatorFactory.GetInstance(typeof(Startup)), DepartmentServiceFactory.Instance,null);
            routeNameProvider = RouteNameProviderFactory.GetRouteNameProvider(ConfigurationFactory.GetConfiguration());

            serviceCommunicator =
                ServiceCommunicatorFactory.GetServiceCommunicator(
                    cacheProvider: InMemoryCacheDataProviderFactory.Instance,
                    credentialProvider: CredentialProviderFactory.GetCredentialProvider(ConfigurationFactory.GetConfiguration()),
                    routeNameProvider: routeNameProvider,
                    serviceRouteRepository: ServiceRouteRepositoryFactory.GetServiceRouteRepository(ConfigurationFactory.GetConfiguration()));
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
            ServiceResultModel<List<DepartmentModel>> serviceResult =
                await
                serviceCommunicator.Call<List<DepartmentModel>>(
                serviceName: routeNameProvider.HR_GetDepartments,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);

            Assert.IsTrue(serviceResult.IsSuccess);
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

            serviceCommunicator.Dispose();
            serviceCommunicator = null;

            routeNameProvider.Dispose();
            routeNameProvider = null;
        }
    }
}
