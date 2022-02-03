using MediatR;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;
using Services.Communication.Http.Broker.Department.HR.Models;
using Services.Logging.Aspect.Handlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Configuration.CQRS.Handlers.QueryHandlers
{
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQueryRequest, GetDepartmentsQueryResponse>
    {
        private readonly RuntimeHandler _runtimeHandler;
        private readonly DepartmentService _departmentService;

        public GetDepartmentsQueryHandler(
            RuntimeHandler runtimeHandler,
            DepartmentService departmentService)
        {
            _runtimeHandler = runtimeHandler;
            _departmentService = departmentService;
        }

        public async Task<GetDepartmentsQueryResponse> Handle(GetDepartmentsQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetDepartmentsQueryResponse()
            {
                Departments =
                await
                _runtimeHandler.ExecuteResultMethod<Task<List<DepartmentModel>>>(
                    _departmentService,
                    nameof(_departmentService.GetDepartmentsAsync),
                    new object[] { new CancellationTokenSource() })
            };
        }
    }
}
