using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses
{
    public class GetPeopleQueryResponse
    {
        public List<PersonModel> People { get; set; }
    }
}
