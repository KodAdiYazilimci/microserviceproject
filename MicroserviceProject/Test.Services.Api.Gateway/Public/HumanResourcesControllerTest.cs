using Infrastructure.Caching.InMemory.Mock;
using Infrastructure.Communication.Http.Broker.Mock;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Security.Authentication.Mock;
using Infrastructure.ServiceDiscovery.Discoverer.Mock;
using Infrastructure.ServiceDiscovery.Mock;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Services.Api.Gateway.Mock.Factories;
using Services.Api.Gateway.Public.Controllers;
using Services.Communication.Http.Broker.Authorization.Mock;
using Services.Communication.Http.Broker.Department.HR.Mock;
using Services.Communication.Http.Broker.Department.HR.Models;
using Services.Communication.Http.Broker.Department.Mock;
using Services.Communication.Http.Broker.Mock;

namespace Test.Services.Api.Gateway.Public
{
    public class HumanResourcesControllerTest
    {
        private HumanResourcesController humanResourcesController;

        public HumanResourcesControllerTest(IConfiguration configuration)
        {
            var defaultCommunicator = DefaultCommunicatorProvider.GetDefaultCommunicator(
                httpGetCaller: HttpGetCallerFactory.Instance,
                httpPostCaller: HttpPostCallerFactory.Instance);

            humanResourcesController =
               new HumanResourcesController
               (
                   apiBridge: ApiBridgeFactory.Instance,
                   hrCommunicator: HRCommunicatorProvider.GetHRCommunicator
                   (
                       departmentCommunicator: DepartmentCommunicatorProvider.GetDepartmentCommunicator
                       (
                           authorizationCommunicator: AuthorizationCommunicatorProvider.GetAuthorizationCommunicator
                           (
                               communicator: defaultCommunicator,
                               serviceDiscoverer: HttpServiceDiscovererProvider.GetServiceDiscoverer(
                                   inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                                   httpGetCaller: HttpGetCallerFactory.Instance,
                                   solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                                   discoveryConfiguration: AppConfigDiscoveryConfigurationProvider.GetDiscoveryConfiguration(configuration))
                           ),
                           inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                           credentialProvider: CredentialProviderFactory.GetCredentialProvider(configuration),
                           communicator: defaultCommunicator
                       ),
                       serviceDiscoverer: HttpServiceDiscovererProvider.GetServiceDiscoverer(
                           inMemoryCacheDataProvider: InMemoryCacheDataProviderFactory.Instance,
                           httpGetCaller: HttpGetCallerFactory.Instance,
                           solidServiceProvider: AppConfigSolidServiceProviderProvider.GetSolidServiceConfiguration(configuration),
                           discoveryConfiguration: AppConfigDiscoveryConfigurationProvider.GetDiscoveryConfiguration(configuration))
                   )
               );
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
            else if (actionResult is BadRequestObjectResult)
            {
                BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)actionResult;

                throw new Exception((badRequestObjectResult.Value as ServiceResultModel).ErrorModel.Description);
            }

            return departments;
        }
    }
}
