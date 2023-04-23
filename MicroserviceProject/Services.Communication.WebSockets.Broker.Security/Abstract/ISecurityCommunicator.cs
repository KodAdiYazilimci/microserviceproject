using Infrastructure.Communication.Http.Models;

using Services.Communication.WebSockets.Broker.Security.CQRS.Requests;
using Services.Communication.WebSockets.Broker.Security.CQRS.Responses;

namespace Services.Communication.WebSockets.Broker.Security.Abstract
{
    public interface ISecurityCommunicator
    {
        Task<ServiceResultModel<SendTokenNotificationResponse>> SendErrorNotificationAsync(
          SendTokenNotificationRequest request,
          string transactionIdentity,
          CancellationTokenSource cancellationTokenSource);
    }
}
