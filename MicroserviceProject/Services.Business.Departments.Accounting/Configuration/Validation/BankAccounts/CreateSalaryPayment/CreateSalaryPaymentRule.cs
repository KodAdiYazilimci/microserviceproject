using Communication.Http.Department.Accounting.Models;

using FluentValidation;

namespace Services.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount
{
    /// <summary>
    /// BankAccounts/CreateSalaryPayment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateSalaryPaymentRule : AbstractValidator<SalaryPaymentModel>
    {
        /// <summary>
        /// BankAccounts/CreateSalaryPayment Http endpoint için validasyon kuralı
        /// </summary>
        public CreateSalaryPaymentRule()
        {
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Miktar boş geçilemez");
            RuleFor(x => x.BankAccount).NotNull().WithMessage("Banka hesabı boş geçilemez");
            RuleFor(x => x.Currency.Id).NotEmpty().WithMessage("Para birimi boş geçilemez");
            RuleFor(x => x.Date).NotNull().WithMessage("Tarih boş geçilemez");
        }
    }
}
