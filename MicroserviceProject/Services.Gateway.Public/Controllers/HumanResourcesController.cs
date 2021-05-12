using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Infrastructure.Transaction.ExecutionHandler;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Gateway.Public.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [Route("HR")]
    public class HumanResourcesController : Controller
    {
        private readonly RouteNameProvider routeNameProvider;
        private readonly ServiceCommunicator serviceCommunicator;

        public HumanResourcesController(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            this.routeNameProvider = routeNameProvider;
            this.serviceCommunicator = serviceCommunicator;
        }

        [HttpGet]
        [Route(nameof(GetDepartments))]        
        public async Task<IActionResult> GetDepartments(CancellationTokenSource cancellationTokenSource)
        {
            return await ServiceExecuter.ExecuteServiceAsync(async () =>
            {
                ServiceResultModel<List<DepartmentModel>> departmentsServiceResult =
                    await
                    serviceCommunicator.Call<List<DepartmentModel>>(
                        serviceName: routeNameProvider.HR_GetDepartments,
                        postData: null,
                        queryParameters: null,
                        cancellationTokenSource: cancellationTokenSource);

                return departmentsServiceResult;
            });
        }
    }
}
