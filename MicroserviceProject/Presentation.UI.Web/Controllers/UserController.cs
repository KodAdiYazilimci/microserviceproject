using Infrastructure.Security.Authentication.Cookie.Providers;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Presentation.UI.Web.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SessionProvider sessionProvider;

        public UserController(SessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        [AllowAnonymous]
        [Route(nameof(Login))]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route(nameof(Login))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] SignUpModel signUpModel, CancellationTokenSource cancellationTokenSource)
        {
            if (ModelState.IsValid)
            {
                bool isLoggedIn = await sessionProvider.LoginAsync(new AuthenticationCredential()
                {
                    Email = signUpModel.Email,
                    Password = signUpModel.Password
                }, cancellationTokenSource);

                if (isLoggedIn)
                {
                    return Redirect("/Home/Index");
                }
            }

            return View();
        }

        [Route(nameof(LogOut))]
        [HttpGet]
        public async Task< IActionResult> LogOut()
        {
            await sessionProvider.LogOutAsync();

            return Redirect("/Login");
        }
    }
}
