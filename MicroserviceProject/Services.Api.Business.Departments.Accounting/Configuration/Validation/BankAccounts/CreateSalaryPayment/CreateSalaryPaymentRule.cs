using Services.Communication.Http.Broker.Department.Accounting.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount
{
    /// <summary>
    /// BankAccounts/CreateSalaryPayment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateSalaryPaymentRule : AbstractValidator<AccountingSalaryPaymentModel>
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
