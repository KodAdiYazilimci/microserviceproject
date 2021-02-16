using FluentValidation;

using MicroserviceProject.Services.Model.Department.HR;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreateWorker
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
        }
    }
}
