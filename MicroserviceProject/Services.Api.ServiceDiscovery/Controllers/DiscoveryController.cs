using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

using Services.Api.ServiceDiscovery.Dto;

namespace Services.Api.ServiceDiscovery.Controllers
{
    [Route("Discovery")]
    public class DiscoveryController : Controller
    {
        private readonly IInMemoryCacheDataProvider _inMemoryCacheDataProvider;
        private readonly IDistrubutedCacheProvider _distrubutedCacheProvider;

        public DiscoveryController(IDistrubutedCacheProvider distrubutedCacheProvider,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            _distrubutedCacheProvider = distrubutedCacheProvider;
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
        }

        [Route("Discover")]
        [HttpGet]
        public IActionResult Discover(string serviceName)
        {
            return HttpResponseWrapper.Wrap(() =>
            {
                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    if (_distrubutedCacheProvider.TryGetValue($"Cached_Services_{serviceName}", out ServiceDto _service) && _service != null)
                    {
                        return _service;
                    }
                }

                return null;
            });
        }

        [Route("DiscoveredServices")]
        [HttpGet]
        public IActionResult GetDiscoveredServices()
        {
            return HttpResponseWrapper.Wrap(() =>
            {
                List<ServiceDto> cachedServices = new List<ServiceDto>();

                List<string> cachedServiceNames = _inMemoryCacheDataProvider.Get<List<string>>("Cached_Services");

                if (cachedServiceNames.Any())
                {
                    foreach (string serviceName in cachedServiceNames)
                    {
                        if (_distrubutedCacheProvider.TryGetValue($"Cached_Services_{serviceName}", out ServiceDto _cachedService) && _cachedService != null)
                        {
                            cachedServices.Add(_cachedService);
                        }
                    }
                }

                return cachedServices;
            });
        }
    }
}
