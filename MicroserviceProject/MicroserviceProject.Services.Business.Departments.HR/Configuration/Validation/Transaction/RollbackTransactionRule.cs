using FluentValidation;

using MicroserviceProject.Services.Transaction.Models;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Transaction
{
    /// <summary>
    /// Transaction/RollbackTransactionRule Http endpoint için validasyon kuralı
    /// </summary>
    public class RollbackTransactionRule : AbstractValidator<RollbackModel>
    {
        /// <summary>
        /// Transaction/RollbackTransactionRule Http endpoint için validasyon kuralı
        /// </summary>
        public RollbackTransactionRule()
        {
            RuleFor(x => x.TransactionIdentity).NotEmpty().WithMessage("İşlem kimliği boş geçilemez");
        }
    }
}
