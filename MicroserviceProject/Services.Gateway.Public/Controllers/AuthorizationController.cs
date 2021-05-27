using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Gateway.Public.Services;
using Services.Gateway.Public.Util.Validation.Auth.GetToken;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Controllers
{
    [Route("Auth")]
    public class AuthorizationController : Controller
    {
        private readonly UserService userService;

        public AuthorizationController(UserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
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

                Token token = await userService.GetTokenAsync(credential.Email, credential.Password, cancellationTokenSource);

                return Ok(new ServiceResultModel<Token>()
                {
                    IsSuccess = true,
                    Data = token
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
    }
}
