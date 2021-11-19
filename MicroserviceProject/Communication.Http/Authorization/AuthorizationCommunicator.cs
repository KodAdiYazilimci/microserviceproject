using Communication.Http.Authorization.Models;

using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Http.Authorization
{
    /// <summary>
    /// Yetki denetimi servisi için iletişim kurucu sınıf
    /// </summary>
    public class AuthorizationCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Yetki denetimi servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public AuthorizationCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Kullanıcı bilgilerine göre token bilgisini verir
        /// </summary>
        /// <param name="credential">Kullanıcı bilgileri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<Token>> GetTokenAsync(Credential credential, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<Token> tokenResult =
                   await _serviceCommunicator.Call<Token>(
                       serviceName: _routeNameProvider.Auth_GetToken,
                       postData: credential,
                       queryParameters: null,
                       headers: null,
                       cancellationTokenSource: cancellationTokenSource);

            return tokenResult;
        }

        /// <summary>
        /// Token bilgisine göre kullanıcı bilgisini verir
        /// </summary>
        /// <param name="headerToken">Token bilgisi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<User>> GetUserAsync(string headerToken, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<User> serviceResult =
                    await
                    _serviceCommunicator.Call<User>(
                        serviceName: _routeNameProvider.Auth_GetUser,
                        postData: null,
                        queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("token", headerToken) },
                        headers: null,
                        cancellationTokenSource: cancellationTokenSource);

            return serviceResult;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _routeNameProvider.Dispose();
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
