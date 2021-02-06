using FluentValidation;

using MicroserviceProject.Services.Business.Departments.Model.Department.HR;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateDepartmentRule : AbstractValidator<DepartmentModel>
    {
        /// <summary>
        /// Department/CreateDepartment Http endpoint için validasyon kuralı
        /// </summary>
        public CreateDepartmentRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Departman adı boş geçilemez");
        }
    }
}
