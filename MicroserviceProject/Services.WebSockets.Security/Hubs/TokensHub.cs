using Infrastructure.Communication.Mq.Rabbit.Models;
using Infrastructure.Security.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Services.WebSockets.Security.Hubs
{
    /// <summary> 
    /// Token mesajları soketi
    /// </summary>
    [Authorize(Policy = "TokensPolicy")]
    public class TokensHub : Hub
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

            await Clients.All.SendAsync("GetTokenMessages", webSocketResultModel);
        }
    }
}
