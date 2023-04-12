using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Services.Api.ServiceDiscovery.Dto;
using Services.Api.ServiceDiscovery.Util.Validation.Registry.Register;

namespace Services.Api.ServiceDiscovery.Controllers
{
    [Route("Registry")]
    public class RegistryController : Controller
    {
        private readonly IDistrubutedCacheProvider _distrubutedCacheProvider;

        public RegistryController(IDistrubutedCacheProvider distrubutedCacheProvider)
        {
            _distrubutedCacheProvider = distrubutedCacheProvider;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] ServiceDto serviceDto)
        {
            return await HttpResponseWrapper.WrapAsync(async delegate
            {
                await RegisterValidator.ValidateAsync(serviceDto, new CancellationTokenSource());

                Dictionary<string, ServiceDto>? services = new Dictionary<string, ServiceDto>();

                if (_distrubutedCacheProvider.TryGetValue("Cached_Services", out string cachedJson) && !string.IsNullOrEmpty(cachedJson))
                {
                    services = JsonConvert.DeserializeObject<Dictionary<string, ServiceDto>>(cachedJson);
                }

                if (services.Any(x => x.Key == serviceDto.ServiceName))
                {
                    services[serviceDto.ServiceName] = serviceDto;
                }
                else
                    services.Add(serviceDto.ServiceName, serviceDto);

                _distrubutedCacheProvider.Set<Dictionary<string, ServiceDto>>("Cached_Services", services, DateTime.UtcNow.AddYears(1));
            });
        }
    }
}
