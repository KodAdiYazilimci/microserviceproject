using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;

namespace Services.Communication.Http.Broker.Department.Accounting.Endpoints
{
    public class GetCurrenciesEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "accounting.bankaccounts.getcurrencies";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
