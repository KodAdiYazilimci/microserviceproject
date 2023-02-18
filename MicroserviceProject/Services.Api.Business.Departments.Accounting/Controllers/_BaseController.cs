using Microsoft.AspNetCore.Mvc;

namespace Services.Api.Business.Departments.Accounting.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Request pipeline da MeditR kullanmayı atlar
        /// </summary>
        public bool ByPassMediatR { get; set; }
    }
}
