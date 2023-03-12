using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Infrastructure.Routing.Providers.Abstract
{
    public interface IRouteProvider
    {
        Task<IEndpoint> GetRoutingEndpointAsync<TEndpoint>(CancellationTokenSource cancellationTokenSource) where TEndpoint : IEndpoint, new();
    }
}
