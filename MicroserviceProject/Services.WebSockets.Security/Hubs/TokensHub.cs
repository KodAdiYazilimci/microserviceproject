using Infrastructure.Security.Model;
using Infrastructure.Sockets.Models;

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
        public async Task SendAsync(AuthenticatedUser user, WebSocketContentModel webSocketMessage)
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
