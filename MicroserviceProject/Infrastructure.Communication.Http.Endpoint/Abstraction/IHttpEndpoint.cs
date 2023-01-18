using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Models;

namespace Infrastructure.Communication.Http.Endpoint.Abstraction
{
    public interface IHttpEndpoint
    {
        string Name { get; set; }
        string Url { get; set; }
        HttpVerb HttpVerb { get; set; }
        List<KeyValuePair<string, string>> Queries { get; set; }
        List<HttpHeader> Headers { get; set; }
    }
}
