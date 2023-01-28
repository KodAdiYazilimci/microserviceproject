using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.Cookie.Handlers;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using Presentation.UI.Web.Models;

using Services.Communication.Http.Endpoints.Presentation.Presentation.UI.Web.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly RouteProvider _routeProvider;
        private readonly CookieHandler _sessionProvider;
        private readonly IConfiguration _configuration;

        public UserController(
            IConfiguration configuration,
            CookieHandler sessionProvider,
            RouteProvider routeProvider)
        {
            _configuration = configuration;
            _sessionProvider = sessionProvider;
            _routeProvider = routeProvider;
        }

        [AllowAnonymous]
        [Route(nameof(Login))]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


            bool useExternalLogin =
                _configuration
                .GetSection("Configuration")
                .GetSection("Authorization")
                .GetSection("Identity")["UseIdentityServerWhileLoggingIn"].ToLower() == "true";

            if (useExternalLogin)
            {
                string redirectInfo = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}/{nameof(LoggedIn)}";

                byte[] redirectInfoAsBytes = System.Text.Encoding.UTF8.GetBytes(redirectInfo);

                string redirectInfoAsBase64 = Convert.ToBase64String(redirectInfoAsBytes);

                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.Add("redirectInfo", redirectInfoAsBase64);

                QueryString queryString = queryBuilder.ToQueryString();

                IEndpoint loginEndpoint = await _routeProvider.GetRoutingEndpointAsync<LoginEndpoint>(cancellationTokenSource);

                string endpoint = loginEndpoint.Url + queryString.Value;

                return Redirect(endpoint);
            }

            return View();
        }

        [Route(nameof(Login))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] SignInModel signInModel, CancellationTokenSource cancellationTokenSource)
        {
            if (ModelState.IsValid)
            {
                bool isLoggedIn = await _sessionProvider.LoginAsync(new AuthenticationCredential()
                {
                    Email = signInModel.Email,
                    Password = signInModel.Password
                }, cancellationTokenSource);

                if (isLoggedIn)
                {
                    return Redirect("/Home/Index");
                }
            }

            return View();
        }

        [Route(nameof(LoggedIn))]
        public async Task<IActionResult> LoggedIn(string token)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (await _sessionProvider.LoginAsync(token, cancellationTokenSource))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        [Route(nameof(LogOut))]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _sessionProvider.LogOutAsync();

            return Redirect("/Login");
        }
    }
}
