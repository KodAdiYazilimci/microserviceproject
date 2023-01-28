using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Infrastructure.Communication.Http.Endpoint.Authentication
{
    public class AnonymouseAuthentication : IEndpointAuthentication
    {
        public void SetAuthentication(HttpClient httpClient)
        {

        }
    }
}
