using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Models;

namespace Infrastructure.Communication.Http.Endpoint.Abstraction
{
    public interface IHttpEndpoint
    {
        string Name { get; }
        string Url { get; }
        HttpVerb HttpVerb { get; }
        List<HttpQuery> Queries { get; }
        List<HttpHeader> Headers { get; }
    }
}
