using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQueryRequest, GetDepartmentsQueryResponse>
    {
        private readonly DepartmentService _departmentService;

        public GetDepartmentsQueryHandler(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<GetDepartmentsQueryResponse> Handle(GetDepartmentsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetDepartmentsQueryResponse()
            {
                Departments = await _departmentService.GetDepartmentsAsync(new CancellationTokenSource())
            };
        }
    }
}
