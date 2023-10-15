using Infrastructure.Validation;

using Services.Api.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment;
using Services.Communication.Http.Broker.Department.HR.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Util.Validation.Department.CreateDepartment
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateDepartmentValidator : BaseValidator<DepartmentModel, CreateDepartmentRule>
    {
        public CreateDepartmentValidator(CreateDepartmentRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(DepartmentModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
