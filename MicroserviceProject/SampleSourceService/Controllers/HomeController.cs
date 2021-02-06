
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;

using Microsoft.AspNetCore.Mvc;

using SampleSourceService.Model;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleSourceService.Controllers
{
    public class HomeController : Controller
    {
        private readonly RouteNameProvider _routeProvider;
        private readonly ServiceCommunicator _serviceCommunicator;

        public HomeController(
            RouteNameProvider routeProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeProvider = routeProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ServiceResult<SampleModel> result = await _serviceCommunicator.Call<SampleModel>(
                serviceName: _routeProvider.SampleDataProvider_GetData,
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("number", "3") },
                cancellationToken: cancellationTokenSource.Token);


            result = await _serviceCommunicator.Call<SampleModel>(
                serviceName: _routeProvider.SampleDataProvider_PostData,
                postData: new SampleModel() { Id = 2, Name = "test 2" },
                queryParameters: null,
                cancellationToken: cancellationTokenSource.Token);

            return Json(result);
        }
    }
}
