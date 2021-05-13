using FluentValidation;

using Infrastructure.Communication.Model.Department.HR;

namespace Services.Business.Departments.HR.Configuration.Validation.Person.CreateWorker
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateWorkerRule : AbstractValidator<WorkerModel>
    {
        /// <summary>
        /// Department/CreateDepartment Http endpoint için validasyon kuralı
        /// </summary>
        public CreateWorkerRule()
        {
            RuleFor(x => x.Person).NotNull().WithMessage("Kişi bilgisi boş geçilemez");
            RuleFor(x => x.BankAccounts).NotNull().WithMessage("Banka hesap bilgisi boş geçilemez");
            RuleFor(x => x.BankAccounts.Count).GreaterThan(0).WithMessage("Banka hesap bilgisi boş geçilemez");
        }
    }
}
