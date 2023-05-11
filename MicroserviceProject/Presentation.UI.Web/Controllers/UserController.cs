using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Security.Authentication.Cookie.Handlers;
using Infrastructure.Security.Model;
using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Presentation.UI.Web.Models;

using Services.Communication.Http.Endpoint.Presentation.UI.Web.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly CookieHandler _sessionProvider;
        private readonly IConfiguration _configuration;
        private readonly IServiceDiscoverer _serviceDiscoverer;

        public UserController(
            IConfiguration configuration,
            CookieHandler sessionProvider,
            IServiceDiscoverer serviceDiscoverer)
        {
            _configuration = configuration;
            _sessionProvider = sessionProvider;
            _serviceDiscoverer = serviceDiscoverer;
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
                CachedServiceModel thisService = await _serviceDiscoverer.GetServiceAsync("Presentation.UI.Web", cancellationTokenSource);

                string redirectInfo = $"{thisService.Protocol}://{thisService.DnsName}:{thisService.Port}/{nameof(LoggedIn)}";

                byte[] redirectInfoAsBytes = System.Text.Encoding.UTF8.GetBytes(redirectInfo);

                string redirectInfoAsBase64 = Convert.ToBase64String(redirectInfoAsBytes);

                QueryBuilder queryBuilder = new QueryBuilder();
                queryBuilder.Add("redirectInfo", redirectInfoAsBase64);

                QueryString queryString = queryBuilder.ToQueryString();

                CachedServiceModel service = await _serviceDiscoverer.GetServiceAsync("Presentation.UI.Web.Identity", cancellationTokenSource);

                IEndpoint endpoint = service.GetEndpoint(LoginEndpoint.Path);

                string endpointUrl = endpoint.Url + queryString.Value;

                return Redirect(endpointUrl);
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
