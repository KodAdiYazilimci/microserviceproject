using Infrastructure.Communication.Http.Models;

using Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Requests;
using Services.Communication.WebSockets.Broker.Reliability.CQRS.Commands.Responses;

namespace Services.Communication.WebSockets.Broker.Reliability.Abstract
{
    public interface IReliabilityCommunicator
    {
        Task<ServiceResultModel<SendErrorNotificationResponse>> SendErrorNotificationAsync(
            SendErrorNotificationRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);
    }
}
