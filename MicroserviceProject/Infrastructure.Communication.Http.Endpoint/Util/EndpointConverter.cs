using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Endpoint.Authentication;
using Infrastructure.Communication.Http.Endpoint.Models;

namespace Infrastructure.Communication.Http.Endpoint.Util
{
    public static class EndpointConverter
    {
        public static IAuthenticatedEndpoint ConvertToAuthenticatedEndpoint(this IEndpoint endpoint)
        {
            return new AuthenticatedEndpoint()
            {
                AuthenticationType = endpoint.AuthenticationType,
                Headers = endpoint.Headers,
                HttpAction = endpoint.HttpAction,
                Name = endpoint.Name,
                Payload = endpoint.Payload,
                Queries = endpoint.Queries,
                StatusCodes = endpoint.StatusCodes,
                Url = endpoint.Url,
                EndpointAuthentication = null
            };
        }

        public static IAuthenticatedEndpoint ConvertToAuthenticatedEndpoint(this IEndpoint endpoint, IEndpointAuthentication endpointAuthentication)
        {
            return new AuthenticatedEndpoint()
            {
                AuthenticationType = endpoint.AuthenticationType,
                Headers = endpoint.Headers,
                HttpAction = endpoint.HttpAction,
                Name = endpoint.Name,
                Payload = endpoint.Payload,
                Queries = endpoint.Queries,
                StatusCodes = endpoint.StatusCodes,
                Url = endpoint.Url,
                EndpointAuthentication = endpointAuthentication
            };
        }
    }
}
