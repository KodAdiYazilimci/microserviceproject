
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Services.Infrastructure.Logging.Util.Validation.Logging.WriteRequestResponseLog;
using MicroserviceProject.Services.Logging.Models;
using MicroserviceProject.Services.Logging.Repositories.Sql;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging.Controllers
{
    [Authorize]
    [Route("Logging")]
    public class LogController : Controller
    {
        private readonly RequestResponseLogRepository _requestResponseRepository;

        public LogController(RequestResponseLogRepository requestResponseRepository)
        {
            _requestResponseRepository = requestResponseRepository;
        }

        [HttpPost]
        [Route(nameof(WriteRequestResponseLog))]
        public async Task<IActionResult> WriteRequestResponseLog([FromBody] RequestResponseLogModel logModel, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResult validationResult = await WriteRequestResponseLogValidator.ValidateAsync(logModel, cancellationToken);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                await _requestResponseRepository.InsertLogAsync(logModel, cancellationToken);

                return Ok(new ServiceResult());
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
