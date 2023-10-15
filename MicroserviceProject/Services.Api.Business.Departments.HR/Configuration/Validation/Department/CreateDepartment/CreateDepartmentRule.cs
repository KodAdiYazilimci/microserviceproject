using FluentValidation;

using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Api.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment
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
