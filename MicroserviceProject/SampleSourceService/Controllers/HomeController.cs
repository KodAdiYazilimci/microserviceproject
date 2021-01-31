using Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration;

using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Moderator;
using MicroserviceProject.Services.Moderator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using SampleSourceService.Model;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleSourceService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ServiceRouteContext _serviceRouteContext;

        public HomeController(
            IMemoryCache memoryCache,
            ServiceRouteContext serviceRouteContext)
        {
            _memoryCache = memoryCache;
            _serviceRouteContext = serviceRouteContext;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ServiceCaller serviceCaller = new ServiceCaller(_memoryCache, "1234");
            serviceCaller.OnNoServiceFoundInCache += (serviceName) =>
            {
                var callModel = (from c in _serviceRouteContext.CallModels
                                 where c.ServiceName == serviceName
                                 select
                                 new CallModel()
                                 {
                                     Id = c.Id,
                                     ServiceName = c.ServiceName,
                                     Endpoint = c.Endpoint,
                                     CallType = c.CallType,
                                     QueryKeys = _serviceRouteContext.QueryKeys.Where(x => x.CallModelId == c.Id).ToList()
                                 }).FirstOrDefault();

                return JsonConvert.SerializeObject(callModel);
            };

            ServiceResult<SampleModel> result = await serviceCaller.Call<SampleModel>(
                serviceName: "sampledataprovider.getdata",
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("number", "3") },
                cancellationToken: cancellationTokenSource.Token);

            result = await serviceCaller.Call<SampleModel>(
                serviceName: "sampledataprovider.postdata",
                postData: new SampleModel() { Id = 2, Name = "test 2" },
                queryParameters: null,
                cancellationToken: cancellationTokenSource.Token);

            return Json(result);
        }
    }
}
