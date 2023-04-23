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

                _distrubutedCacheProvider.Set<ServiceDto>($"Cached_Services_{serviceDto.ServiceName}", serviceDto, DateTime.UtcNow.AddYears(1));
            });
        }
    }
}
