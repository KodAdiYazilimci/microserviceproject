using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Infrastructure.Communication.Http.Endpoint.Authentication
{
    public class TokenAuthentication : IEndpointAuthentication
    {
        string token = string.Empty;
        public TokenAuthentication(string token)
        {
            this.token = token;
        }

        public void SetAuthentication(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
        }
    }
}
