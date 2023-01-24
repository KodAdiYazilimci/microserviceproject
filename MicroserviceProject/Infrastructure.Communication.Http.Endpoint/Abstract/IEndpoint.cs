using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Models;

namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IEndpoint
    {
        string Url { get; set; }
        string Name { get; set; }
        object Payload { get; set; }
        HttpAction HttpAction { get; set; }
        List<HttpHeader> Headers { get; set; }
        List<HttpQuery> Queries { get; set; }
        IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
