using Infrastructure.Transaction.Recovery;
using Infrastructure.Validation;

using Services.Api.Business.Departments.CR.Configuration.Validation.Transaction;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Util.Validation.Transaction
{
    /// <summary>
    /// Transaction/RollbackTransaction Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class RollbackTransactionValidator : BaseValidator<RollbackModel, RollbackTransactionRule>
    {
        public RollbackTransactionValidator(RollbackTransactionRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(RollbackModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
