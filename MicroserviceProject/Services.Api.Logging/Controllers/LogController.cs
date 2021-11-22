using Infrastructure.Communication.Http.Models;
using Infrastructure.Logging.Logger.RequestResponseLogger;
using Infrastructure.Logging.Logger.RequestResponseLogger.Persistence;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Infrastructure.Logging.Util.Validation.Logging.WriteRequestResponseLog;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Infrastructure.Logging.Controllers
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
        public async Task<IActionResult> WriteRequestResponseLog([FromBody] RequestResponseLogModel logModel, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                ServiceResultModel validationResult = await WriteRequestResponseLogValidator.ValidateAsync(logModel, cancellationTokenSource);

                if (!validationResult.IsSuccess)
                {
                    return BadRequest(validationResult);
                }

                await _requestResponseRepository.InsertLogAsync(logModel, cancellationTokenSource);

                return Ok(new ServiceResultModel());
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = ex.ToString() }
                });
            }
        }
    }
}
