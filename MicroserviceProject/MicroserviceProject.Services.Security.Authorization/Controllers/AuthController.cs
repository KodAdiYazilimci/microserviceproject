using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Model.Security;
using MicroserviceProject.Services.Security.Authorization.Persistence.Providers;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly SqlDataProvider authorizationDataProvider;

        public AuthController(
            SqlDataProvider authorizationDataProvider)
        {
            this.authorizationDataProvider = authorizationDataProvider;
        }

        [Route("GetToken")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] Credential credential)
        {
            if (credential == null || string.IsNullOrEmpty(credential.Email) || string.IsNullOrEmpty(credential.Password))
            {
                throw new Exception("Geçersiz kullanıcı adı ve parola ikilisi", null);
            }

            var token = await authorizationDataProvider.GetTokenAsync(credential, new CancellationTokenSource().Token);

            return Ok(new ServiceResult<Token>()
            {
                IsSuccess = true,
                Data = token
            });
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string token)
        {
            try
            {
                User user = await authorizationDataProvider.GetUserAsync(token, new CancellationTokenSource().Token);

                return Ok(new ServiceResult<User>()
                {
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResult(isSuccess: false) { Error = new Error() { Description = ex.ToString() } });
            }
        }

        [Route("CheckUser")]
        [HttpGet]
        public async Task<IActionResult> CheckUser(string email)
        {
            return Ok(new ServiceResult<bool>()
            {
                Data = await authorizationDataProvider.CheckUserAsync(email, new CancellationTokenSource().Token)
            });
        }

        [Route("RegisterUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] Credential credential)
        {
            await authorizationDataProvider.RegisterUserAsync(credential, new CancellationTokenSource().Token);

            return Ok(new ServiceResult());
        }
    }
}
