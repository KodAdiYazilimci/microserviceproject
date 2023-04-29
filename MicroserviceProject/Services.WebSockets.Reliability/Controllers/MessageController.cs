using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Security.Model;
using Infrastructure.Sockets.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Services.WebSockets.Reliability.Hubs;

using System.Threading.Tasks;

namespace Services.WebSockets.Reliability.Controllers
{
    [Route("")]
    public class MessageController : Controller
    {
        private readonly ErrorHub _tokensHub;

        public MessageController(ErrorHub tokensHub)
        {
            _tokensHub = tokensHub;
        }

        [Route(nameof(SendErrorNotification))]
        [HttpPost]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser,WebPresentationUser")]
        public async Task<IActionResult> SendErrorNotification([FromBody] WebSocketContentModel webSocketMessage)
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
