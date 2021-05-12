using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;
using MicroserviceProject.Infrastructure.Validation.Exceptions;
using MicroserviceProject.Infrastructure.Validation.Model;
using MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreateTitle;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateTitle
{
    /// <summary>
    /// Person/CreateTitle Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateTitleValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="title">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(TitleModel title, CancellationTokenSource cancellationTokenSource)
        {
            CreateTitleRule validationRules = new CreateTitleRule();

            if (title != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(title, cancellationTokenSource.Token);

                if (!validationResult.IsValid)
                {
                    ValidationModel validation = new ValidationModel()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItemModel>()
                    };

                    validation.ValidationItems.AddRange(
                        validationResult.Errors.Select(x => new ValidationItemModel()
                        {
                            Key = x.PropertyName,
                            Value = x.AttemptedValue,
                            Message = x.ErrorMessage
                        }).ToList());

                    throw new ValidationException(validation);
                }
            }
            else
            {
                ValidationModel validation = new ValidationModel()
                {
                    IsValid = false,
                    ValidationItems = new List<ValidationItemModel>()
                };

                throw new ValidationException(validation);
            }
        }
    }
}
