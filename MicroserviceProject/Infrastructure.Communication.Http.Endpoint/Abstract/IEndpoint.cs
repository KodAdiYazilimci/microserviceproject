using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Constants;
using Infrastructure.Communication.Http.Models;

using System.Net;

namespace Infrastructure.Communication.Http.Endpoint.Abstract
{
    public interface IEndpoint
    {
        static string Path { get; set; }
        string Url { get; set; }
        string Name { get; set; }
        object? Payload { get; set; }
        HttpAction HttpAction { get; set; }
        List<HttpHeaderModel> Headers { get; set; }
        List<HttpQueryModel> Queries { get; set; }
        AuthenticationType AuthenticationType { get; set; }
        EndpointPurpose EndpointPurpose { get; set; }
        List<HttpStatusCode> StatusCodes { get; set; }
    }
}
