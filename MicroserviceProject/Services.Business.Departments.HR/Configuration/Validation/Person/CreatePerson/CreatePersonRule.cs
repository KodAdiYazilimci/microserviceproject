using FluentValidation;

using Services.Business.Departments.HR.Models;

namespace Services.Business.Departments.HR.Configuration.Validation.Person.CreatePerson
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralı
    /// </summary>
    public class CreatePersonRule : AbstractValidator<PersonModel>
    {
        /// <summary>
        /// Department/CreateDepartment Http endpoint için validasyon kuralı
        /// </summary>
        public CreatePersonRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kişi adı boş geçilemez");
        }
    }
}
