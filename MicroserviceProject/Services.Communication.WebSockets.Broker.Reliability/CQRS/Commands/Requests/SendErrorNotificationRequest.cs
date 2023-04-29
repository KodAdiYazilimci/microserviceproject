using Infrastructure.Sockets.Models;

using MediatR;

using Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Responses;

namespace Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Requests
{
    public class SendErrorNotificationRequest : IRequest<SendErrorNotificationResponse>
    {
        public WebSocketContentModel WebSocketContent { get; set; }
    }
}
