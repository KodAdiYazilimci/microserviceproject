using MicroserviceProject.Services.Business.Departments.HR.Services;
using MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Transaction;
using MicroserviceProject.Services.Transaction.Models;

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
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationToken cancellationToken)
        {
            return await ServiceExecuter.ExecuteServiceAsync<int>(async () =>
            {
                await RollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationToken);

                int rollbackResult = 0;

                if (rollbackModel.Modules.Contains(_departmentService.ServiceName))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                if (rollbackModel.Modules.Contains(_personService.ServiceName))
                {
                    rollbackResult = await _departmentService.RollbackTransactionAsync(rollbackModel, cancellationToken);
                }

                return rollbackResult;
            },
            services: new BaseService[] { _personService, _departmentService });
        }
    }
}