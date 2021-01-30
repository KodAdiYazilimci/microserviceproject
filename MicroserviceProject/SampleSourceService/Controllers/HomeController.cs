using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Infrastructure.Communication.Http.Providers;

using Microsoft.AspNetCore.Mvc;

using SampleSourceService.Model;

using System.Threading;
using System.Threading.Tasks;
using MicroserviceProject.Services.Moderator;
using Newtonsoft.Json;
using MicroserviceProject.Services.Moderator.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace SampleSourceService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //HttpGetProvider httpGetProvider = new HttpGetProvider();

            //ServiceResult<SampleModel> getResult = await httpGetProvider.GetAsync<ServiceResult<SampleModel>>("http://localhost:15269/GetData", cancellationTokenSource.Token);

            //HttpPostProvider httpPostProvider = new HttpPostProvider();

            //ServiceResult<SampleModel> result = await httpPostProvider.PostAsync<ServiceResult<SampleModel>, SampleModel>("http://localhost:15269/PostData", new SampleModel()
            //{
            //    Id = 2,
            //    Name = "test 2"
            //}, cancellationTokenSource.Token);

            ServiceCaller serviceCaller = new ServiceCaller(_memoryCache, "1234");

            serviceCaller.OnNoServiceFoundInCache += delegate
            {
                return JsonConvert.SerializeObject(new CallModel()
                {
                    CallType = "GET",
                    Endpoint = "http://localhost:15269/GetData",
                    QueryKeys = new List<string>(),
                    ServiceName = "sampledataprovider.getdata"
                });
            };

            ServiceResult<SampleModel> result = await serviceCaller.Call<SampleModel>("sampledataprovider.getdata", null, null, cancellationTokenSource.Token);

            serviceCaller = new ServiceCaller(_memoryCache, "1234");

            serviceCaller.OnNoServiceFoundInCache += delegate
            {
                return JsonConvert.SerializeObject(new CallModel()
                {
                    CallType = "POST",
                    Endpoint = "http://localhost:15269/PostData",
                    QueryKeys = new List<string>(),
                    ServiceName = "sampledataprovider.getdata"
                });
            };

            result = await serviceCaller.Call<SampleModel>("sampledataprovider.postdata", new SampleModel()
            {
                Id = 2,
                Name = "test 2"
            }, null, cancellationTokenSource.Token);


            return Json(result);
        }
    }
}
