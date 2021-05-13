using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Route("SendTokenNotification")]
        public async Task<IActionResult> SendTokenNofication([FromBody] string message)
        {
            await _tokensHub.SendAsync(message);

            return Ok();
        }
    }
}
