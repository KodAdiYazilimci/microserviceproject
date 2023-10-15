using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Transaction.Recovery;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Api.Business.Departments.HR.Services;
using Services.Api.Business.Departments.HR.Util.Validation.Transaction;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Controllers
{
    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private readonly DepartmentService _departmentService;
        private readonly PersonService _personService;
        private readonly RollbackTransactionValidator _rollbackTransactionValidator;

        public TransactionController(
            DepartmentService departmentService,
            PersonService personService,
            RollbackTransactionValidator rollbackTransactionValidator)
        {
            _departmentService = departmentService;
            _personService = personService;
            _rollbackTransactionValidator = rollbackTransactionValidator;
        }

        [HttpPost]
        [Route(nameof(RollbackTransaction))]
        [Authorize(Roles = "ApiUser")]
        public async Task<IActionResult> RollbackTransaction([FromBody] RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            return await HttpResponseWrapper.WrapAsync<int>(async () =>
            {
                await _rollbackTransactionValidator.ValidateAsync(rollbackModel, cancellationTokenSource);

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