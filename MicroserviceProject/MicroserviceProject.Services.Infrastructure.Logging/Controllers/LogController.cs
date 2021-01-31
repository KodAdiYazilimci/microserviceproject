using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Model.Logging;
using MicroserviceProject.Services.Infrastructure.Logging.Logging.Loggers;

using Microsoft.AspNetCore.Mvc;

using System;

namespace MicroserviceProject.Services.Infrastructure.Logging.Controllers
{
    [Route("Logging")]
    public class LogController : Controller
    {
        private readonly RequestResponseLogger _requestResponseLogManager;

        public LogController(RequestResponseLogger requestResponseLogManager)
        {
            _requestResponseLogManager = requestResponseLogManager;
        }

        [HttpPost]
        [Route(nameof(WriteRequestResponseLog))]
        public IActionResult WriteRequestResponseLog([FromBody] BaseLogModel logModel)
        {
            try
            {
                return Json(new ServiceResult());
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() }
                });
            }
        }
    }
}
