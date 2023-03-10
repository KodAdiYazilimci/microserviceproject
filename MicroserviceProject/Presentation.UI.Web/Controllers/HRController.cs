using AutoMapper;

using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Communication.Http.Broker.Gateway.Public.Abstract;
using Services.Communication.Http.Broker.Gateway.Public.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    [Authorize]
    [Route("HR")]
    public class HRController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IPublicGatewayCommunicator _publicGatewayCommunicator;

        public HRController(
            IMapper mapper,
            IPublicGatewayCommunicator publicGatewayCommunicator)
        {
            _mapper = mapper;
            _publicGatewayCommunicator = publicGatewayCommunicator;
        }

        [Route(nameof(Departments))]
        public async Task<IActionResult> Departments()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult = 
                await _publicGatewayCommunicator.GetDepartmentsAsync(
                    transactionIdentity: Guid.NewGuid().ToString(),
                    cancellationTokenSource: cancellationTokenSource);

            if (departmentsServiceResult.IsSuccess)
            {
                List<Models.HR.DepartmentModel> departmentModels = 
                    _mapper.Map<List<DepartmentModel>, List<Models.HR.DepartmentModel>>(departmentsServiceResult.Data);

                ViewBag.Error = null;
                return View(departmentModels);
            }
            else
            {
                ViewBag.Error = "Hata:" + departmentsServiceResult.ErrorModel?.Description;
                return View();
            }
        }
    }
}
