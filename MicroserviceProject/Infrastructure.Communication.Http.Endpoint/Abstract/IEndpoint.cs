using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Models;

namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IEndpoint
    {
        string Url { get; set; }
        string Name { get; set; }
        object? Payload { get; set; }
        HttpAction HttpAction { get; set; }
        Dictionary<string, string> Headers { get; set; }
        Dictionary<string, string> Queries { get; set; }
        IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
