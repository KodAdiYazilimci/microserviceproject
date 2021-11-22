using AutoMapper;

using Services.Communication.Http.Broker.Gateway.Public;
using Services.Communication.Http.Broker.Gateway.Public.Models;

using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        private readonly HRCommunicator _hRCommunicator;

        public HRController(
            IMapper mapper,
            HRCommunicator hRCommunicator)
        {
            _mapper = mapper;
            _hRCommunicator = hRCommunicator;
        }

        [Route(nameof(Departments))]
        public async Task<IActionResult> Departments()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult = 
                await _hRCommunicator.GetDepartmentsAsync(
                    transactionIdentity: Guid.NewGuid().ToString(),
                    cancellationTokenSource: cancellationTokenSource);

            if (departmentsServiceResult.IsSuccess)
            {
                List<Models.HR.DepartmentModel> departmentModels = 
                    _mapper.Map<List<DepartmentModel>, List<Models.HR.DepartmentModel>>(departmentsServiceResult.Data);

                return View(departmentModels);
            }
            else
            {
                return View();
            }
        }
    }
}
