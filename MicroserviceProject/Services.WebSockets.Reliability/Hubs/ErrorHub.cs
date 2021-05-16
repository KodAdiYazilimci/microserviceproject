using Infrastructure.Communication.Model.Basics;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Services.WebSockets.Reliability.Hubs
{
    /// <summary> 
    /// Hata mesajları soketi
    /// </summary>
    [Authorize(Policy = "ErrorPolicy")]
    public class ErrorHub : Hub
    {
        /// <summary>
        /// Sokete mesaj gönderir
        /// </summary>
        /// <param name="user">Sokete gönderilen mesajın sahibi</param>
        /// <param name="webSocketMessage">Soket mesajın içeriği</param>
        /// <returns></returns>
        public async Task SendAsync(User user, WebSocketContentModel webSocketMessage)
        {
            WebSocketResultModel webSocketResultModel = new WebSocketResultModel()
            {
                Sender = user,
                Content = webSocketMessage
            };

            await Clients.All.SendAsync("GetErrorMessages", webSocketResultModel);
        }
    }
}
