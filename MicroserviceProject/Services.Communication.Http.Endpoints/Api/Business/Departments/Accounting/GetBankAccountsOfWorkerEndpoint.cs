using Infrastructure.Communication.Http.Constants;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;

namespace Services.Communication.Http.Endpoints.Api.Business.Departments.Accounting
{
    public class GetBankAccountsOfWorkerEndpoint : IEndpoint
    {
        public string Url { get; set; }
        public string Name { get; set; } = "accounting.bankaccounts.getbankaccountsofworker";
        public object Payload { get; set; }
        public HttpAction HttpAction { get; set; } = HttpAction.GET;
        public List<HttpHeader> Headers { get; set; }
        public List<HttpQuery> Queries { get; set; }
        public IEndpointAuthentication EndpointAuthentication { get; set; }
    }
}
