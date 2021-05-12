using MicroserviceProject.Infrastructure.Transaction.ExecutionHandler;
using MicroserviceProject.Infrastructure.Transaction.Recovery;
using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Transaction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly PersonService _personService;

        public TransactionController(
            DepartmentService departmentService,
            PersonService personService)
        {
            _departmentService = departmentService;
            _personService = personService;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            if (Request != null && Request.Headers.ContainsKey("TransactionIdentity"))
            {
                _departmentService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
                _personService.TransactionIdentity = Request.Headers["TransactionIdentity"].ToString();
            }

            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_departmentService.ServiceName))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationTokenSource);
                }

                if (rollbackModel.Modules.Contains(_personService.ServiceName))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationTokenSource);
                }

                return rollbackResult;
            },
            services: new BaseService[] { _personService, _departmentService });
        }
    }
}