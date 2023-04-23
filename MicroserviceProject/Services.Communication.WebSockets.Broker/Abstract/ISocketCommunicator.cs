using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.WebSockets.Broker.Abstract
{
    public interface ISocketCommunicator
    {
        Task<ServiceResultModel<TResult>> CallAsync<TResult>(IAuthenticatedEndpoint endpoint, CancellationTokenSource cancellationTokenSource);
        Task<ServiceResultModel<TResult>> CallAsync<TRequest, TResult>(IAuthenticatedEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource);
        Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource);
    }
}
