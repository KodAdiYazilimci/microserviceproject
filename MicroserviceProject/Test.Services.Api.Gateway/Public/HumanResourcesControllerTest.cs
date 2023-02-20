using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Persistence.Mock;
using Infrastructure.Routing.Providers.Mock;
using Infrastructure.Security.Authentication.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Services.Api.Gateway.Mock.Factories;
using Services.Api.Gateway.Public.Controllers;
using Services.Communication.Http.Broker.Authorization.Mock;
using Services.Communication.Http.Broker.Department.HR.Mock;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Test.Services.Api.Gateway.Public
{
    public class HumanResourcesControllerTest
    {
        private HumanResourcesController humanResourcesController;

        public HumanResourcesControllerTest(IConfiguration configuration)
        {
            humanResourcesController =
               new HumanResourcesController(
            apiBridge: ApiBridgeFactory.Instance,
                   hrCommunicator: HRCommunicatorProvider.GetHRCommunicator(
                       AuthorizationCommunicatorProvider.GetAuthorizationCommunicator(
                           HttpGetCallerFactory.Instance,
                           HttpPostCallerFactory.Instance,
                           RouteProviderFactory.GetRouteProvider(
                               ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration),
            InMemoryCacheDataProviderFactory.Instance)),
                       InMemoryCacheDataProviderFactory.Instance,
                       CredentialProviderFactory.GetCredentialProvider(configuration),
                       HttpGetCallerFactory.Instance,
                       HttpPostCallerFactory.Instance,
                       RouteProviderFactory.GetRouteProvider(
                           ServiceRouteRepositoryFactory.GetServiceRouteRepository(configuration),
                           InMemoryCacheDataProviderFactory.Instance)));
        }
        public async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            List<DepartmentModel> departments = null;

            IActionResult actionResult = await humanResourcesController.GetDepartments(new CancellationTokenSource());

            if (actionResult is OkObjectResult)
            {
                OkObjectResult okObjectResult = (OkObjectResult)actionResult;

                departments = (okObjectResult.Value as ServiceResultModel<List<DepartmentModel>>).Data;
            }

            return departments;
        }
    }
}
