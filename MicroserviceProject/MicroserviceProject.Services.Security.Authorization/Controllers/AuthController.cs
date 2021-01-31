using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Model.Communication.Validations;
using MicroserviceProject.Model.Security;
using MicroserviceProject.Services.Security.Authorization.Business.Services;
using MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Exceptions;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Controllers
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
        public async Task<IActionResult> GetToken([FromBody] Credential credential)
        {
            try
            {
                if (credential == null || string.IsNullOrEmpty(credential.Email) || string.IsNullOrEmpty(credential.Password))
                {
                    ServiceResult serviceResult = new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = "Geçersiz kullanıcı adı ve parola ikilisi"
                        },
                        Validation = new Validation()
                        {
                            IsValid = false,
                            ValidationItems = new List<ValidationItem>()
                        }
                    };

                    if (credential == null)
                    {
                        serviceResult.Validation.ValidationItems.Add(new ValidationItem() { Key = nameof(Credential), Value = null });
                    }

                    if (string.IsNullOrEmpty(credential.Email))
                    {
                        serviceResult.Validation.ValidationItems.Add(new ValidationItem() { Key = nameof(Credential.Email), Value = credential.Email });
                    }

                    if (string.IsNullOrEmpty(credential.Password))
                    {
                        serviceResult.Validation.ValidationItems.Add(new ValidationItem() { Key = nameof(Credential.Password), Value = credential.Password });
                    }

                    return BadRequest(serviceResult);
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
