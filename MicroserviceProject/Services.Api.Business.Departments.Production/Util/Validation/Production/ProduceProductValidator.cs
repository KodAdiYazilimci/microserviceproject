using Infrastructure.Validation;

using Services.Api.Business.Departments.Production.Configuration.Validation.Production;
using Services.Communication.Http.Broker.Department.Production.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Util.Validation.Production
{
    /// <summary>
    /// Production/ProduceProduct Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class ProduceProductValidator : BaseValidator<ProduceModel, ProduceProductRule>
    {
        public ProduceProductValidator(ProduceProductRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(ProduceModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
