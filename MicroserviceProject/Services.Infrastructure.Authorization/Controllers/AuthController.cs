﻿using Infrastructure.Communication.Model.Basics;
using Infrastructure.Communication.Model.Errors;
using Infrastructure.Security.Model;
using Services.Infrastructure.Authorization.Business.Services;
using Services.Infrastructure.Authorization.Persistence.Sql.Exceptions;
using Services.Infrastructure.Authorization.Util.Validation.Auth.GetToken;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Controllers
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
        public async Task<IActionResult> GetToken([FromBody] Credential credential, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                var validateResult = await GetTokenValidator.ValidateAsync(credential, cancellationTokenSource);

                if (!validateResult.IsSuccess)
                {
                    return BadRequest(validateResult);
                }

                var token = await _sessionService.GetTokenAsync(credential, new CancellationTokenSource());

                return Ok(new ServiceResultModel<Token>()
                {
                    IsSuccess = true,
                    Data = token
                });
            }
            catch (UserNotFoundException unf)
            {
                return Unauthorized(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = unf.ToString() }
                });
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

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string token)
        {
            try
            {
                User user = await _userService.GetUserAsync(token, new CancellationTokenSource());

                return Ok(new ServiceResultModel<User>()
                {
                    Data = user
                });
            }
            catch (UserNotFoundException unf)
            {
                return Unauthorized(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = unf.ToString() }
                });
            }
            catch (SessionNotFoundException snf)
            {
                return Unauthorized(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = snf.ToString() }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResultModel() { IsSuccess = false, ErrorModel = new ErrorModel() { Description = ex.ToString() } });
            }
        }

        [Route("CheckUser")]
        [HttpGet]
        public async Task<IActionResult> CheckUser(string email)
        {
            try
            {
                return Ok(new ServiceResultModel<bool>()
                {
                    Data = await _userService.CheckUserAsync(email, new CancellationTokenSource())
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new ServiceResultModel() { IsSuccess = false, ErrorModel = new ErrorModel() { Description = ex.ToString() } });
            }
        }

        [Route("RegisterUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] Credential credential)
        {
            try
            {
                await _userService.RegisterUserAsync(credential, new CancellationTokenSource());

                return Ok(new ServiceResultModel()
                {
                    IsSuccess = true
                });
            }
            catch (UserNotFoundException unf)
            {
                return BadRequest(new ServiceResultModel() { IsSuccess = false, ErrorModel = new ErrorModel() { Description = unf.ToString() } });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResultModel() { IsSuccess = false, ErrorModel = new ErrorModel() { Description = ex.ToString() } });
            }
        }
    }
}