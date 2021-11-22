using Services.Communication.Http.Broker.Department.HR.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.HR.Configuration.Validation.Person.CreateTitle
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
