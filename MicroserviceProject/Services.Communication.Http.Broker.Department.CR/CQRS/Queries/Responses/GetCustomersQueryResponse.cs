using Services.Communication.Http.Broker.Department.CR.Models;

namespace Services.Communication.Http.Broker.Department.CR.CQRS.Queries.Responses
{
    public class GetCustomersQueryResponse
    {
        public List<CustomerModel> Customers { get; set; }
    }
}
