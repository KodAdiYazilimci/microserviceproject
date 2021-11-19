using Infrastructure.Security.Authentication.Cookie.Providers;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Presentation.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionProvider sessionProvider;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            SessionProvider sessionProvider)
        {
            _logger = logger;
            this.sessionProvider = sessionProvider;
        }

        [Authorize(Roles = "StandardUser")]
        public async Task<IActionResult> Index(CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser user = await sessionProvider.GetLoggedInUserAsyc(cancellationTokenSource);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [Route(nameof(Yetkisiz))]
        public IActionResult Yetkisiz()
        {
            return View();
        }
    }
}
