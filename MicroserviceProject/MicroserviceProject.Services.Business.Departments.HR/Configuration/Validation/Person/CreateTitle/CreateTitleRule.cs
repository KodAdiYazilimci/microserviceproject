using FluentValidation;

using MicroserviceProject.Services.Business.Model.Department.HR;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreateTitle
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateTitleRule : AbstractValidator<TitleModel>
    {
        /// <summary>
        /// Department/CreateDepartment Http endpoint için validasyon kuralı
        /// </summary>
        public CreateTitleRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ünvan adı boş geçilemez");
        }
    }
}
