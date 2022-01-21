using Infrastructure.Communication.Http.Wrapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Services;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Requests;
using Services.Communication.Http.Broker.Department.HR.CQRS.Queries.Responses;

using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Controllers
{
    [Route("Department")]
    public class DepartmentController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly DepartmentService _departmentService;

        public DepartmentController(IMediator mediator, DepartmentService departmentService)
        {
            _mediator = mediator;
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route(nameof(GetDepartments))]
        [Authorize(Roles = "ApiUser,GatewayUser")]
        public async Task<IActionResult> GetDepartments()
        {
            return await HttpResponseWrapper.WrapAsync<GetDepartmentsQueryResponse>(async () =>
            {
                return await _mediator.Send(new GetDepartmentsQueryRequest());
            },
            services: _departmentService);
        }

        [HttpPost]
        [Route(nameof(CreateDepartment))]
        [Authorize(Roles = "ApiUser,GatewayUser,QueueUser")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommandRequest request)
        {
            return await HttpResponseWrapper.WrapAsync<CreateDepartmentCommandResponse>(async () =>
            {
                return await _mediator.Send(request);
            },
            services: _departmentService);
        }
    }
}
