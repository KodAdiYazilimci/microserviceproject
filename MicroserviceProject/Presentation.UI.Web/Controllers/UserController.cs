using Microsoft.AspNetCore.Mvc;

using Presentation.UI.Web.Models;

using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    public class UserController : Controller
    {
        //private readonly SignInManager<User> signInManager;

        //public UserController(SignInManager<User> signInManager)
        //{
        //    this.signInManager = signInManager;
        //}

        [Route(nameof(Login))]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route(nameof(Login))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] CredentialModel credential)
        {
            if (ModelState.IsValid)
            {
                //await signInManager.SignInAsync(new Infrastructure.Security.Model.User()
                //{
                //    Email = credential.Email,
                //    Password = credential.Password
                //}, isPersistent: true, authenticationMethod: AuthenticationMethods);

                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return View();
        }
    }
}
