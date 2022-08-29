using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.WebSockets.Models;
using Infrastructure.Routing.Exceptions;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.Repositories.Sql;
using Infrastructure.Security.Authentication.Exceptions;
using Infrastructure.Security.Authentication.Providers;
using Infrastructure.Security.Model;
using Infrastructure.Sockets.Exceptions;
using Infrastructure.Sockets.Models;
using Infrastructure.Sockets.Persistence.Repositories.Sql;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.WebSockets
{
    /// <summary>
    /// Bir websocket bağlantısını dinleyen sınıf
    /// </summary>
    public class SocketListener : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Servis endpointlerinin önbellekteki adı
        /// </summary>
        private const string CACHEDSERVICEROUTES = "CACHED_SERVICE_ROUTES";

        /// <summary>
        /// Websocket ile iletişimde kullanılacak yetki tokenının önbellekteki saklama anahtarı
        /// </summary>
        private const string TAKENTOKENFORTHISSERVICE = "TAKEN_TOKEN_FOR_THIS_SERVICE";

        /// <summary>
        /// Servis endpointlerinin önbellekteki adı
        /// </summary>
        private const string CACHEDWEBSOCKETS = "CACHED_WEBSOCKETS";

        /// <summary>
        /// Önbellek nesnesi
        /// </summary>
        private readonly InMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// İletişimde kullanılacak yetkiler için sağlayıcı
        /// </summary>
        private readonly CredentialProvider _credentialProvider;

        /// <summary>
        /// Servis endpointleri sağlayıcısı
        /// </summary>
        private readonly ServiceRouteRepository _serviceRouteRepository;

        /// <summary>
        /// Soket endpointlerinin sağlayıcısı
        /// </summary>
        private readonly SocketRepository _socketRepository;

        /// <summary>
        /// Gelen soket verisini yakalayacak handler
        /// </summary>
        /// <param name="webSocketResult">Yakalanan soket verisi</param>
        public delegate void OnMessageReceivedHandler(WebSocketResultModel webSocketResult);

        /// <summary>
        /// Soketten veri alındığında ateşlenecek olay
        /// </summary>
        public event OnMessageReceivedHandler OnMessageReceived;

        /// <summary>
        /// Bir websocket bağlantısını dinleyen sınıf
        /// </summary>
        /// <param name="cacheProvider">Önbellek nesnesi</param>
        /// <param name="credentialProvider">İletişimde kullanılacak yetkiler için sağlayıcı</param>
        /// <param name="serviceRouteRepository">Servis endpointleri sağlayıcısı</param>
        /// <param name="socketRepository">Soket endpointlerinin sağlayıcısı</param>
        public SocketListener(
            InMemoryCacheDataProvider cacheProvider,
            CredentialProvider credentialProvider,
            ServiceRouteRepository serviceRouteRepository,
            SocketRepository socketRepository)
        {
            _cacheProvider = cacheProvider;
            _credentialProvider = credentialProvider;
            _serviceRouteRepository = serviceRouteRepository;
            _socketRepository = socketRepository;
        }

        /// <summary>
        /// Bir web soketi dinlemeye başlar
        /// </summary>
        /// <param name="socketName">Dinlenecek soketin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task ListenAsync(string socketName, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticationToken takenTokenForThisService = _cacheProvider.Get<AuthenticationToken>(TAKENTOKENFORTHISSERVICE);

            if (string.IsNullOrWhiteSpace(takenTokenForThisService?.TokenKey)
                ||
                takenTokenForThisService.ValidTo <= DateTime.Now)
            {
                ServiceCaller serviceTokenCaller = new ServiceCaller(_cacheProvider, "");
                serviceTokenCaller.OnNoServiceFoundInCacheAsync += async (serviceName) =>
                {
                    return await GetServiceAsync(serviceName, cancellationTokenSource);
                };

                ServiceResultModel<AuthenticationToken> tokenResult =
                    await serviceTokenCaller.Call<AuthenticationToken>(
                        serviceName: "authorization.auth.gettoken",
                        postData: new AuthenticationCredential()
                        {
                            Email = _credentialProvider.GetEmail,
                            Password = _credentialProvider.GetPassword
                        },
                        queryParameters: null,
                        headers: null,
                        cancellationTokenSource: cancellationTokenSource);

                if (tokenResult.IsSuccess && tokenResult.Data != null)
                {
                    takenTokenForThisService = tokenResult.Data;
                    _cacheProvider.Set<AuthenticationToken>(TAKENTOKENFORTHISSERVICE, tokenResult.Data);
                }
                else
                {
                    throw new GetTokenException("Kaynak servis yetki tokenı elde edilemedi");
                }
            }

            SocketModel socket = await GetSocketAsync(socketName, cancellationTokenSource);

            HubConnection hubConnection = new HubConnectionBuilder().WithUrl(socket.Endpoint, options =>
            {
                options.Headers.Add("Authorization", takenTokenForThisService.TokenKey);
            }).Build();

            hubConnection.On<object>(socket.Method, param =>
            {
                if (OnMessageReceived != null)
                {
                    WebSocketResultModel webSocketResult = JsonConvert.DeserializeObject<WebSocketResultModel>(param.ToString());

                    OnMessageReceived(webSocketResult);
                }
            });

            await hubConnection.StartAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Servis rota bilgisini verir
        /// </summary>
        /// <param name="serviceName">Bilgisi getirilecek servisin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        private async Task<string> GetServiceAsync(string serviceName, CancellationTokenSource cancellationTokenSource)
        {
            List<ServiceRouteModel> serviceRoutes = _cacheProvider.Get<List<ServiceRouteModel>>(CACHEDSERVICEROUTES);

            if (serviceRoutes == null || !serviceRoutes.Any())
            {
                serviceRoutes = await _serviceRouteRepository.GetServiceRoutesAsync(cancellationTokenSource);

                _cacheProvider.Set<List<ServiceRouteModel>>(CACHEDSERVICEROUTES, serviceRoutes, DateTime.Now.AddMinutes(60));
            }

            if (serviceRoutes.Any(x => x.ServiceName == serviceName))
                return JsonConvert.SerializeObject(serviceRoutes.FirstOrDefault(x => x.ServiceName == serviceName));
            else
                throw new GetRouteException("Servis rotası bulunamadı");
        }

        /// <summary>
        /// Soket bilgisini verir
        /// </summary>
        /// <param name="socketName">Bilgisi getirilecek soketin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        private async Task<SocketModel> GetSocketAsync(string socketName, CancellationTokenSource cancellationTokenSource)
        {
            List<SocketModel> sockets = _cacheProvider.Get<List<SocketModel>>(CACHEDWEBSOCKETS);

            if (sockets == null || !sockets.Any())
            {
                sockets = await _socketRepository.GetSocketsAsync(cancellationTokenSource);

                _cacheProvider.Set<List<SocketModel>>(CACHEDWEBSOCKETS, sockets, DateTime.Now.AddMinutes(60));
            }

            if (sockets.Any(x => x.Name == socketName))
                return sockets.FirstOrDefault(x => x.Name == socketName);
            else
                throw new GetSocketException("Soket bilgisi bulunamadı");
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
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (_credentialProvider != null)
                        _credentialProvider.Dispose();

                    if (_serviceRouteRepository != null)
                        _serviceRouteRepository.Dispose();

                    if (_socketRepository != null)
                        _socketRepository.Dispose();
                }

                disposed = true;
            }
        }
    }
}
