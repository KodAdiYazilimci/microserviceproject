using Infrastructure.Validation;

using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreateWorker;
using Services.Communication.Http.Broker.Department.HR.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Util.Validation.Person.CreateWorker
{
    /// <summary>
    /// Person/CreateWorker Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateWorkerValidator : BaseValidator<WorkerModel, CreateWorkerRule>
    {
        public CreateWorkerValidator(CreateWorkerRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(WorkerModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
