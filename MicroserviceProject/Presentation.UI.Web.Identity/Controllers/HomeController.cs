using Microsoft.AspNetCore.Mvc;

namespace Presentation.UI.Web.Identity.Controllers
{
    public class HomeController : Controller
    {
        [Route(nameof(Hata))]
        public IActionResult Hata()
        {
            return View();
        }
    }
}
