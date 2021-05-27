using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Routing.Providers;
using Infrastructure.Security.Authentication.SignalR.Requirements;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security.Authentication.SignalR.Handlers
{
    /// <summary>
    /// Varsayılan kimlik denetimi yapan sınıf
    /// </summary>
    public class DefaultAuthorizationHandler : AuthorizationHandler<DefaultAuthorizationRequirement>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Önbellekte tutulacak token bazlı kullanıcı oturumları için önbellek anahtarı
        /// </summary>
        private const string CACHEDTOKENBASEDSESSIONS = "CACHED_TOKENBASED_SESSIONS";

        /// <summary>
        /// Http üst öğelerine erişim sağlayacak nesne
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Oturum bilgilerinin saklanacağı önbellek nesnesi
        /// </summary>
        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// Yetki sunucusu adreslerini sağlayacak rota sağlayıcı nesnes
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki sunucusuyla iletişim kuracak servis iletişimcisi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Varsayılan kimlik denetimi yapan sınıf
        /// </summary>
        /// <param name="httpContextAccessor">Http üst öğelerine erişim sağlayacak nesne</param>
        /// <param name="cacheProvider">Oturum bilgilerinin saklanacağı önbellek nesnesi</param>
        /// <param name="routeNameProvider">Yetki sunucusu adreslerini sağlayacak rota sağlayıcı nesnes</param>
        /// <param name="serviceCommunicator">Yetki sunucusuyla iletişim kuracak servis iletişimcisi</param>
        public DefaultAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            InMemoryCacheDataProvider cacheProvider,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _httpContextAccessor = httpContextAccessor;
            _cacheProvider = cacheProvider;
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultAuthorizationRequirement requirement)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            User user = GetUserFromCache(token);

            if (user != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    ServiceResultModel<User> serviceResult =
                        await
                        _serviceCommunicator.Call<User>(
                            serviceName: _routeNameProvider.Auth_GetUser,
                            postData: null,
                            queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("token", token) },
                            headers: null,
                            cancellationTokenSource: cancellationTokenSource);

                    if (serviceResult.IsSuccess && serviceResult.Data != null)
                    {
                        SetToCache(serviceResult.Data);

                        context.Succeed(requirement);
                    }
                    else
                        context.Fail();
                }
                else
                {
                    context.Fail();
                }
            }
        }

        /// <summary>
        /// Token bazlı kullanıcı oturumunu önbellekte saklar
        /// </summary>
        /// <param name="userModel">Kullanıcının model nesnesi</param>
        private void SetToCache(User userModel)
        {
            if (_cacheProvider.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<User> cachedUsers) && cachedUsers != default(List<User>))
            {
                cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now);

                cachedUsers.Add(userModel);

                _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
            }
            else
            {
                List<User> userModels = new List<User>
                {
                    userModel
                };

                _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, userModels);
            }
        }

        /// <summary>
        /// Bir kullanıcının geçerli token bazlı oturum bilgisini önbellekten çağırır
        /// </summary>
        /// <param name="token">Kullanıcının oturum anahtarı</param>
        /// <returns></returns>
        private User GetUserFromCache(string token)
        {
            if (_cacheProvider.TryGetValue(CACHEDTOKENBASEDSESSIONS, out List<User> cachedUsers) && cachedUsers != default(List<User>))
            {
                if (cachedUsers.RemoveAll(x => x == null || x.Token == null || x.Token.ValidTo < DateTime.Now) > 0)
                {
                    _cacheProvider.Set(CACHEDTOKENBASEDSESSIONS, cachedUsers);
                }

                return cachedUsers.FirstOrDefault(x => x.Token.TokenKey == token);
            }

            return null;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (_routeNameProvider != null)
                        _routeNameProvider.Dispose();

                    if (_serviceCommunicator != null)
                        _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
