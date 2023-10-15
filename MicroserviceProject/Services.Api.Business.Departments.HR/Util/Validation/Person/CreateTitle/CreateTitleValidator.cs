using Infrastructure.Validation;

using Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreateTitle;
using Services.Communication.Http.Broker.Department.HR.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Util.Validation.Person.CreateTitle
{
    /// <summary>
    /// Person/CreateTitle Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateTitleValidator : BaseValidator<TitleModel, CreateTitleRule>
    {
        public CreateTitleValidator(CreateTitleRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(TitleModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
