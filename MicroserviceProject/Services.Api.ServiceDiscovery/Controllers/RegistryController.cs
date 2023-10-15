using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Wrapper;

using Microsoft.AspNetCore.Mvc;

using Services.Api.ServiceDiscovery.Dto;
using Services.Api.ServiceDiscovery.Util.Validation.Registry.Register;

namespace Services.Api.ServiceDiscovery.Controllers
{
    [Route("Registry")]
    public class RegistryController : Controller
    {
        private readonly IInMemoryCacheDataProvider _inMemoryCacheDataProvider;
        private readonly IDistrubutedCacheProvider _distrubutedCacheProvider;
        private readonly RegisterValidator _registerValidator;

        public RegistryController(IDistrubutedCacheProvider distrubutedCacheProvider,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider,
            RegisterValidator registerValidator)
        {
            _distrubutedCacheProvider = distrubutedCacheProvider;
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
            _registerValidator = registerValidator;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] ServiceDto serviceDto)
        {
            return await HttpResponseWrapper.WrapAsync(async delegate
            {
                await _registerValidator.ValidateAsync(serviceDto, new CancellationTokenSource());

                _distrubutedCacheProvider.Set<ServiceDto>($"Cached_Services_{serviceDto.ServiceName}", serviceDto, DateTime.UtcNow.AddYears(1));

                List<string> discoveredServices = new List<string>();

                if (_inMemoryCacheDataProvider.TryGetValue<List<string>>("Cached_Services", out List<string> _discoveredServices))
                {
                    discoveredServices = _discoveredServices;
                }

                discoveredServices.Add(serviceDto.ServiceName);

                _inMemoryCacheDataProvider.Set<List<string>>("Cached_Services", discoveredServices);
            });
        }

        [Route("DropService")]
        [HttpGet]
        public IActionResult DropService(string serviceName)
        {
            return HttpResponseWrapper.Wrap(() =>
            {
                _distrubutedCacheProvider.RemoveObject($"Cached_Services_{serviceName}");

                List<string> discoveredServices = new List<string>();

                if (_inMemoryCacheDataProvider.TryGetValue<List<string>>("Cached_Services", out List<string> _discoveredServices))
                {
                    discoveredServices = _discoveredServices;
                }

                discoveredServices.Remove(serviceName);

                _inMemoryCacheDataProvider.Set<List<string>>("Cached_Services", discoveredServices);
            });
        }
    }
}
