using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses
{
    public class GetTitlesQueryResponse
    {
        public List<TitleModel> Titles { get; set; }
    }
}
