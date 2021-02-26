using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Routing.Persistence.Repositories.Sql;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Persistence;
using MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Schemes;
using MicroserviceProject.Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Handlers
{
    /// <summary>
    /// Kimlik doğrulama denetimi yapacak sınıf
    /// </summary>
    public class MasterAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";
        private const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";

        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;
        private readonly ServiceRouteRepository _serviceRouteRepository;

        /// <summary>
        /// Kimlik doğrulama denetimi yapacak sınıf
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="urlEncoder"></param>
        /// <param name="systemClock"></param>
        public MasterAuthentication(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock,
            IMemoryCache memoryCache,
            RouteNameProvider routeNameProvider,
            ServiceRouteRepository serviceRouteRepository,
            ServiceCommunicator serviceCommunicator,
            IConfiguration configuration
            ) : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
            _routeNameProvider = routeNameProvider;
            _serviceRouteRepository = serviceRouteRepository;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Kimlik denetimini sağlar
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (await GetUserAsync(cancellationTokenSource.Token) != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(AuthenticationPersistence.GetClaims(await GetUserAsync(cancellationTokenSource.Token)), Default.DefaultScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Default.DefaultScheme));
            }
            else
            {
                return AuthenticateResult.Fail("Lütfen tekrar oturum açın!");
            }
        }

        /// <summary>
        /// Oturumda bulunan kullanıcı
        /// </summary>
        protected async Task<User> GetUserAsync(CancellationToken cancellationToken)
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues headerToken) && headerToken.Any(x => x.Length > 0))
            {
                User user = GetUserFromCache(headerToken);

                if (user != null)
                {
                    return user;
                }              

                ServiceResultModel<User> serviceResult =
                    await
                    _serviceCommunicator.Call<User>(
                        serviceName: _routeNameProvider.Auth_GetUser,
                        postData: null,
                        queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("token", headerToken) },
                        cancellationToken: cancellationToken);

                if (serviceResult.IsSuccess && serviceResult.Data != null)
                {
                    SetToCache(serviceResult.Data);
                }

                return serviceResult.Data;
            }

            //throw new SessionExpiredException("Lütfen tekrar oturum açın!");
            return null;
        }

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        private void SetToCache(User userModel)
        {
            if (_memoryCache.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<User> cachedUsers) && cachedUsers != default(List<User>))
            {
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo == null || x.Token.ValidTo < DateTime.Now);

                cachedUsers.Add(userModel);

                _memoryCache.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
            }
            else
            {
                List<User> userModels = new List<User>
                {
                    userModel
                };

                _memoryCache.Set(CACHEDTOKENBASEDSESSIONS, userModels);
            }
        }

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        private User GetUserFromCache(string token)
        {
            if (_memoryCache.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<User> cachedUsers) && cachedUsers != default(List<User>))
            {
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo == null || x.Token.ValidTo < DateTime.Now) > 0)
                {
                    _memoryCache.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token);
            }

            return null;
        }
    }
}
