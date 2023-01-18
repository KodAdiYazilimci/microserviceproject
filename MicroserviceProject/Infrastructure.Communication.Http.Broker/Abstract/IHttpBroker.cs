using Infrastructure.Communication.Http.Endpoint.Abstraction;
using Infrastructure.Communication.Http.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Http.Broker.Abstract
{
    public interface IHttpBroker
    {

    }

    public interface IHttpBroker<TResult> : IHttpBroker where TResult : class, new()
    {
        Task<ServiceResultModel<TResult>> Call(IHttpEndpoint httpEndpoint, string transactionIdentity, CancellationTokenSource cancellationTokenSource);
    }

    public interface IHttpBroker<TRequestEntity, TResult> : IHttpBroker where TRequestEntity : class, new() where TResult : class, new()
    {
        Task<ServiceResultModel<TResult>> Call(TRequestEntity request, IHttpEndpoint httpEndpoint, string transactionIdentity, CancellationTokenSource cancellationTokenSource);
    }
}
