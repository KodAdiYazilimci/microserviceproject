using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.WebSockets.Models;
using Infrastructure.Security.Authentication.Abstract;
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
        private const string CACHEDWEBSOCKETS = "CACHED_WEBSOCKETS";

        /// <summary>
        /// Önbellek nesnesi
        /// </summary>
        private readonly IInMemoryCacheDataProvider _cacheProvider;

        /// <summary>
        /// İletişimde kullanılacak yetkiler için sağlayıcı
        /// </summary>
        private readonly ICredentialProvider _credentialProvider;

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
            IInMemoryCacheDataProvider cacheProvider,
            ICredentialProvider credentialProvider,
            SocketRepository socketRepository)
        {
            _cacheProvider = cacheProvider;
            _credentialProvider = credentialProvider;
            _socketRepository = socketRepository;
        }

        /// <summary>
        /// Bir web soketi dinlemeye başlar
        /// </summary>
        /// <param name="socketName">Dinlenecek soketin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task ListenAsync(string socketName, string token, CancellationTokenSource cancellationTokenSource)
        {
            SocketModel socket = await GetSocketAsync(socketName, cancellationTokenSource);

            HubConnection hubConnection = new HubConnectionBuilder().WithUrl(socket.Endpoint, options =>
            {
                options.Headers.Add("Authorization", token);
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

                _cacheProvider.Set<List<SocketModel>>(CACHEDWEBSOCKETS, sockets, DateTime.UtcNow.AddMinutes(60));
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

                    if (_socketRepository != null)
                        _socketRepository.Dispose();
                }

                disposed = true;
            }
        }
    }
}
