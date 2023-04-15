namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IAuthenticatedEndpoint : IEndpoint
    {
        IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
