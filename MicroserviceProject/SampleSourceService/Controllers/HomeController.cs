using MicroserviceProject.Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration;

using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Moderator;
using MicroserviceProject.Model.Security;
using MicroserviceProject.Services.Moderator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

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
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly ServiceRouteContext _serviceRouteContext;

        public HomeController(
            IConfiguration configuration,
            IMemoryCache memoryCache,
            ServiceRouteContext serviceRouteContext)
        {
            _configuration = configuration;
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

            Token takenTokenForThisService = _memoryCache.Get<Token>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCache += (serviceName) =>
                {
                    return GetServiceFromInMemoryDB(serviceName);
                };
                ServiceResult<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: "authorization.gettoken",
                        postData: new Credential()
                        {
                            Email = _configuration.GetSection("Configuration").GetSection("Credential").GetSection("email").Value,
                            Password = _configuration.GetSection("Configuration").GetSection("Credential").GetSection("password").Value
                        },
                        queryParameters: null,
                        cancellationToken: cancellationTokenSource.Token);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _memoryCache.Set<Token>(TAKENTOKENFORTHISSERVICE, tokenResult.Data);
                }
                else
                {
                    throw new Exception("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            ServiceCaller serviceCaller = new ServiceCaller(_memoryCache, takenTokenForThisService.TokenKey);
            serviceCaller.OnNoServiceFoundInCache += (serviceName) =>
            {
                return GetServiceFromInMemoryDB(serviceName);
            };

            ServiceResult<SampleModel> result = await serviceCaller.Call<SampleModel>(
                serviceName: "sampledataprovider.getdata",
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("number", "3") },
                cancellationToken: cancellationTokenSource.Token);

            //result = await serviceCaller.Call<SampleModel>(
            //    serviceName: "sampledataprovider.postdata",
            //    postData: new SampleModel() { Id = 2, Name = "test 2" },
            //    queryParameters: null,
            //    cancellationToken: cancellationTokenSource.Token);

            return Json(result);
        }

        private string GetServiceFromInMemoryDB(string serviceName)
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
        }
    }
}
