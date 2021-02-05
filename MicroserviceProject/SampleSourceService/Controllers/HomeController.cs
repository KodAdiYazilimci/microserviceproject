
using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Infrastructure.Security.Model;

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
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";
        private const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";

        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly ServiceRouteRepository _serviceRouteRepository;
        private readonly RouteProvider _routeProvider;

        public HomeController(
            IConfiguration configuration,
            IMemoryCache memoryCache,
            ServiceRouteRepository serviceRouteRepository,
            RouteProvider routeProvider)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
            _serviceRouteRepository = serviceRouteRepository;
            _routeProvider = routeProvider;
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
                CredentialProvider credentialProvider = new CredentialProvider(_configuration);

                ServiceCaller serviceTokenCaller = new ServiceCaller(_memoryCache, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationTokenSource.Token);
                };
                ServiceResult<Token> tokenResult =
                    await serviceTokenCaller.Call<Token>(
                        serviceName: _routeProvider.Auth_GetToken,
                        postData: new Credential()
                        {
                            Email = credentialProvider.GetEmail,
                            Password = credentialProvider.GetPassword
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
            serviceCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
            {
                return await GetServiceAsync(serviceName, cancellationTokenSource.Token);
            };

            ServiceResult<SampleModel> result = await serviceCaller.Call<SampleModel>(
                serviceName: _routeProvider.SampleDataProvider_GetData,
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("number", "3") },
                cancellationToken: cancellationTokenSource.Token);

            result = await serviceCaller.Call<SampleModel>(
                serviceName: _routeProvider.SampleDataProvider_PostData,
                postData: new SampleModel() { Id = 2, Name = "test 2" },
                queryParameters: null,
                cancellationToken: cancellationTokenSource.Token);

            return Json(result);
        }

        /// <summary>
        /// Servis rota bilgisini verir
        /// </summary>
        /// <param name="serviceName">Bilgisi getirilecek servisin adı</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        private async Task<string> GetServiceAsync(string serviceName, CancellationToken cancellationToken)
        {
            List<ServiceRoute> serviceRoutes = _memoryCache.Get<List<ServiceRoute>>(CACHEDSERVICEROUTES);

            if (serviceRoutes == null || !serviceRoutes.Any())
            {
                serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationToken);

                return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
            }

            serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationToken);

            _memoryCache.Set<List<ServiceRoute>>(CACHEDSERVICEROUTES, serviceRoutes, DateTime.Now.AddMinutes(60));

            return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
        }
    }
}
