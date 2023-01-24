namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IEndpointAuthentication
    {
        void SetAuthentication(HttpClient httpClient);
    }
}
