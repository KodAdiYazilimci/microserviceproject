using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Broker.Abstract
{
    public interface ICommunicator
    {
        Task<ServiceResultModel<TResult>> CallAsync<TResult>(IEndpoint endpoint, CancellationTokenSource cancellationTokenSource);
        Task<ServiceResultModel<TResult>> CallAsync<TRequest, TResult>(IEndpoint endpoint, TRequest requestObject, CancellationTokenSource cancellationTokenSource);
    }
}
