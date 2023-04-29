using Infrastructure.Sockets.Models;

using MediatR;

using Services.Communication.WebSockets.Broker.Security.CQRS.Responses;

namespace Services.Communication.WebSockets.Broker.Security.CQRS.Requests
{
    public class SendTokenNotificationRequest : IRequest<SendTokenNotificationResponse>
    {
        public WebSocketContentModel WebSocketContent { get; set; }
    }
}
