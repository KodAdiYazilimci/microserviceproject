using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Infrastructure.Security.Model;
using MicroserviceProject.Services.Infrastructure.Authorization.Business.Services;
using MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using MicroserviceProject.Services.Infrastructure.Authorization.Util.Validation.Auth.GetToken;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly SessionService _sessionService;
        private readonly UserService _userService;

        public AuthController(
            SessionService sessionService,
            UserService userService)
        {
            _sessionService = sessionService;
            _userService = userService;
        }

        [Route("GetToken")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] Credential credential, CancellationToken cancellationToken)
        {
            try
            {
                var validateResult = await GetTokenValidator.ValidateAsync(credential, cancellationToken);

                if (!validateResult.IsSuccess)
                {
                    return BadRequest(validateResult);
                }

                var token = await _sessionService.GetTokenAsync(credential, new CancellationTokenSource().Token);

                return Ok(new ServiceResult<Token>()
                {
                    IsSuccess = true,
                    Data = token
                });
            }
            catch (UserNotFoundException unf)
            {
                return Unauthorized(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = unf.ToString() }
                });
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

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string token)
        {
            try
            {
                User user = await _userService.GetUserAsync(token, new CancellationTokenSource().Token);

                return Ok(new ServiceResult<User>()
                {
                    Data = user
                });
            }
            catch (UserNotFoundException unf)
            {
                return Unauthorized(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = unf.ToString() }
                });
            }
            catch (SessionNotFoundException snf)
            {
                return Unauthorized(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = snf.ToString() }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult() { IsSuccess = false, Error = new Error() { Description = ex.ToString() } });
            }
        }

        [Route("CheckUser")]
        [HttpGet]
        public async Task<IActionResult> CheckUser(string email)
        {
            try
            {
                return Ok(new ServiceResult<bool>()
                {
                    Data = await _userService.CheckUserAsync(email, new CancellationTokenSource().Token)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new ServiceResult() { IsSuccess = false, Error = new Error() { Description = ex.ToString() } });
            }
        }

        [Route("RegisterUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] Credential credential)
        {
            try
            {
                await _userService.RegisterUserAsync(credential, new CancellationTokenSource().Token);

                return Ok(new ServiceResult()
                {
                    IsSuccess = true
                });
            }
            catch (UserNotFoundException unf)
            {
                return BadRequest(new ServiceResult() { IsSuccess = false, Error = new Error() { Description = unf.ToString() } });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult() { IsSuccess = false, Error = new Error() { Description = ex.ToString() } });
            }
        }
    }
}
