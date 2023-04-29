using Infrastructure.Security.Authentication.Abstract;
using Infrastructure.Sockets.Abstract;
using Infrastructure.Sockets.Models;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

namespace Services.WebSockets
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
        /// İletişimde kullanılacak yetkiler için sağlayıcı
        /// </summary>
        private readonly ICredentialProvider _credentialProvider;

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
        public SocketListener(ICredentialProvider credentialProvider)
        {
            _credentialProvider = credentialProvider;
        }

        /// <summary>
        /// Bir web soketi dinlemeye başlar
        /// </summary>
        /// <param name="socketName">Dinlenecek soketin adı</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task ListenAsync(IWebSocketEndpoint socketEndpoint, string token, CancellationTokenSource cancellationTokenSource)
        {
            HubConnection hubConnection = new HubConnectionBuilder().WithUrl(socketEndpoint.Endpoint, options =>
            {
                options.Headers.Add("Authorization", token);
            }).Build();

            hubConnection.On<object>(socketEndpoint.Method, param =>
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
                }

                disposed = true;
            }
        }
    }
}
