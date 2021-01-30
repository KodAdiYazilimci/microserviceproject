using MicroserviceProject.Common.Model.Communication.Basics;
using MicroserviceProject.Infrastructure.Communication.Http.Providers;

using Microsoft.AspNetCore.Mvc;

using SampleSourceService.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleSourceService.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        [Route("Index")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            HttpGetProvider httpGetProvider = new HttpGetProvider();

            ServiceResult<SampleModel> getResult = await httpGetProvider.GetAsync<ServiceResult<SampleModel>>("http://localhost:15269/GetData", cancellationTokenSource.Token);

            HttpPostProvider httpPostProvider = new HttpPostProvider();

            ServiceResult<SampleModel> result = await httpPostProvider.PostAsync<ServiceResult<SampleModel>, SampleModel>("http://localhost:15269/PostData", new SampleModel()
            {
                Id = 2,
                Name = "test 2"
            }, cancellationTokenSource.Token);

            return Json(result);
        }
    }
}
