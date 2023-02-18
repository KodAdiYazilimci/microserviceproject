using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

namespace Services.Api.Business.Departments.HR.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Request pipeline da MeditR kullanmayı atlar
        /// </summary>
        public bool ByPassMediatR { get; set; }
    }
}
