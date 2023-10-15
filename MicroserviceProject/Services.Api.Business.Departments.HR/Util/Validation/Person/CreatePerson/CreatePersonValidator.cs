using Infrastructure.Validation;

using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreatePerson;
using Services.Communication.Http.Broker.Department.HR.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Util.Validation.Person.CreatePerson
{
    /// <summary>
    /// Person/CreatePerson Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreatePersonValidator : BaseValidator<PersonModel, CreatePersonRule>
    {
        public CreatePersonValidator(CreatePersonRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(PersonModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
