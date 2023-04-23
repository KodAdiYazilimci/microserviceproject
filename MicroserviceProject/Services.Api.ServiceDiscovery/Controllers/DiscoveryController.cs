using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

using Services.Api.ServiceDiscovery.Dto;

namespace Services.Api.ServiceDiscovery.Controllers
{
    [Route("Discovery")]
    public class DiscoveryController : Controller
    {
        private readonly IDistrubutedCacheProvider _distrubutedCacheProvider;

        public DiscoveryController(IDistrubutedCacheProvider distrubutedCacheProvider)
        {
            _distrubutedCacheProvider = distrubutedCacheProvider;
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
    }
}
