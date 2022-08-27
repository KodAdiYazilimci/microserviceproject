using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Gateway.Public.Util.Communication;
using Services.Communication.Http.Broker.Department.HR;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Gateway.Public.Controllers
{
    [Route("HR")]
    public class HumanResourcesController : Controller
    {
        private readonly ApiBridge _apiBridge;
        private readonly HRCommunicator _hrCommunicator;

        public HumanResourcesController(
            ApiBridge apiBridge,
            HRCommunicator hrCommunicator)
        {
            _apiBridge = apiBridge;
            _hrCommunicator = hrCommunicator;
        }

        [HttpGet]
        [Route(nameof(GetDepartments))]
        [Authorize(Roles = "WebPresentationUser")]
        public async Task<IActionResult> GetDepartments(CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<List<DepartmentModel>>(async () =>
            {
                return await _apiBridge.CallAsync<List<DepartmentModel>>(async (transactionIdentity, cancellationTokenSource) =>
                {
                    return await _hrCommunicator.GetDepartmentsAsync(transactionIdentity, cancellationTokenSource);
                }, cancellationTokenSource);
            });
        }
    }
}
