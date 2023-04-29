using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Security.Model;
using Infrastructure.Sockets.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Services.WebSockets.Security.Hubs;

using System.Threading.Tasks;

namespace Services.WebSockets.Security.Controllers
{
    [Route("")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly TokensHub _tokensHub;

        public MessageController(TokensHub tokensHub)
        {
            _tokensHub = tokensHub;
        }

        [Route(nameof(SendTokenNotification))]
        [HttpPost]
        public async Task<IActionResult> SendTokenNotification([FromBody] WebSocketContentModel webSocketMessage)
        {
            return await HttpResponseWrapper.WrapAsync(async () =>
            {
                AuthenticatedUser user = null;

                foreach (var claim in this.User.Claims)
                {
                    if (claim.Value != "User")
                    {
                        user = JsonConvert.DeserializeObject<AuthenticatedUser>(claim.Value);
                        break;
                    }
                }
                await _tokensHub.SendAsync(user, webSocketMessage);
            });
        }
    }
}
