using FluentValidation;

using Services.Business.Departments.Accounting.Models;

namespace Services.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount
{
    /// <summary>
    /// BankAccounts/CreateCurrency Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateCurrencyRule : AbstractValidator<CurrencyModel>
    {
        /// <summary>
        /// BankAccounts/CreateBankAccount Http endpoint için validasyon kuralı
        /// </summary>
        public CreateCurrencyRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş geçilemez");
            RuleFor(x => x.ShortName).NotEmpty().WithMessage("Kısa isim boş geçilemez");
        }
    }
}
