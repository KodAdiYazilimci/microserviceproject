using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Broker.Abstract
{
    public interface ICommunicator
    {
        Task<ServiceResultModel<TResult>> CallAsync<TResult>(IAuthenticatedEndpoint endpoint, CancellationTokenSource cancellationTokenSource);
        Task<ServiceResultModel<TResult>> CallAsync<TRequest, TResult>(IAuthenticatedEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource);
    }
}
