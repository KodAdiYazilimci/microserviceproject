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
            if (!string.IsNullOrEmpty(serviceName))
            {
                return HttpResponseWrapper.Wrap(() =>
                {

                    if (_distrubutedCacheProvider.TryGetValue("Cached_Services", out List<ServiceDto> services) && services != null)
                    {
                        return services.FirstOrDefault(x => x.ServiceName == serviceName);
                    }

                    return null;
                });
            }
            else
            {
                return HttpResponseWrapper.Wrap(() =>
                {
                    if (_distrubutedCacheProvider.TryGetValue("Cached_Services", out List<ServiceDto> services) && services != null)
                    {
                        return services;
                    }

                    return new List<ServiceDto>();
                });
            }
        }
    }
}
