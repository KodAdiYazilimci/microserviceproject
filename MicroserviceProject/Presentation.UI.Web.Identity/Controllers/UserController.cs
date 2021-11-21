using Communication.Http.Authorization;
using Communication.Http.Authorization.Models;

using Infrastructure.Communication.Http.Broker.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using Presentation.UI.Web.Identity.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Identity.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthorizationCommunicator _authorizationCommunicator;

        public UserController(
            AuthorizationCommunicator authorizationCommunicator)
        {
            _authorizationCommunicator = authorizationCommunicator;
        }

        [Route(nameof(Login))]
        public IActionResult Login(string redirectInfo)
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([FromForm] SignInModel signInModel, string redirectInfo)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            byte[] redirectInfoAsBytes = Convert.FromBase64String(redirectInfo);

            string endpoint = System.Text.Encoding.UTF8.GetString(redirectInfoAsBytes);

            ServiceResultModel<TokenModel> tokenServiceResult = await _authorizationCommunicator.GetTokenAsync(new CredentialModel()
            {
                Email = signInModel.Email,
                Password = signInModel.Password,
            }, cancellationTokenSource);

            if (tokenServiceResult.IsSuccess)
            {
                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.Add("token", tokenServiceResult.Data.TokenKey);

                QueryString queryString = queryBuilder.ToQueryString();

                endpoint += queryString.Value;

                return Redirect(endpoint);
            }
            else
            {
                return View();
            }
        }
    }
}
