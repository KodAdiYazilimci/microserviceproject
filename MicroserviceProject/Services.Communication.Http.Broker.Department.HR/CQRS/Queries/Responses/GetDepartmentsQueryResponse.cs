using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses
{
    public class GetDepartmentsQueryResponse
    {
        public List<DepartmentModel> Departments { get; set; }
    }
}
