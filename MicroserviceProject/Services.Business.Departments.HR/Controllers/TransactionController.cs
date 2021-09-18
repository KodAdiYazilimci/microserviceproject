using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;
using Services.Business.Departments.HR.Services;
using Services.Business.Departments.HR.Util.Validation.Transaction;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.HR.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : BaseController
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
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
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