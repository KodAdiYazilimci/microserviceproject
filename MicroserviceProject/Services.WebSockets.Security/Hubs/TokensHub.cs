using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Services.WebSockets.Security.Hubs
{
    [Authorize(Policy = "TokensPolicy")]
    public class TokensHub : Hub
    {
        public async Task SendAsync(string incomingMessage)
        {
            await Clients.All.SendAsync("GetTokenMessages", incomingMessage);
        }
    }
}
