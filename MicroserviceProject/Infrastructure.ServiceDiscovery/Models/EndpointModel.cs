using Infrastructure.Communication.Http.Constants;
using Infrastructure.ServiceDiscovery.Constants;

using System.Net;

namespace Infrastructure.ServiceDiscovery.Models
{
    public class EndpointModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public HttpAction HttpAction { get; set; }
        public List<HttpHeaderModel> Headers { get; set; }
        public List<HttpQueryModel> Queries { get; set; }
        public EndpointAuthentications EndpointAuthentication { get; set; }
        public List<HttpStatusCode> StatusCodes { get; set; }
    }
}
