using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses
{
    public class GetWorkersQueryResponse
    {
        public List<WorkerModel> Workers { get; set; }
    }
}
