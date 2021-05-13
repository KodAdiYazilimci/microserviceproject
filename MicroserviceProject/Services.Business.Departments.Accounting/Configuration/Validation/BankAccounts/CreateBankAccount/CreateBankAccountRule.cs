using FluentValidation;

using Infrastructure.Communication.Model.Department.Accounting;

namespace Services.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount
{
    /// <summary>
    /// BankAccounts/CreateBankAccount Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateBankAccountRule : AbstractValidator<BankAccountModel>
    {
        /// <summary>
        /// BankAccounts/CreateBankAccount Http endpoint için validasyon kuralı
        /// </summary>
        public CreateBankAccountRule()
        {
            RuleFor(x => x.IBAN).NotEmpty().WithMessage("IBAN boş geçilemez");
            RuleFor(x => x.Worker).NotNull().WithMessage("Çalışan bilgisi boş geçilemez");
        }
    }
}
