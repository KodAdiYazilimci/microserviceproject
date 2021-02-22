using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Moderator.Exceptions;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Validations;
using MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreateTitle;
using MicroserviceProject.Services.Model.Department.HR;

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
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(TitleModel title, CancellationToken cancellationToken)
        {
            CreateTitleRule validationRules = new CreateTitleRule();

            if (title != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(title, cancellationToken);

                if (!validationResult.IsValid)
                {
                    ServiceResult serviceResult = new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = "Geçersiz parametre"
                        },
                        Validation = new Infrastructure.Communication.Moderator.Model.Validations.Validation()
                        {
                            IsValid = false,
                            ValidationItems = new List<ValidationItem>()
                        }
                    };
                    serviceResult.Validation.ValidationItems.AddRange(
                        validationResult.Errors.Select(x => new ValidationItem()
                        {
                            Key = x.PropertyName,
                            Value = x.AttemptedValue,
                            Message = x.ErrorMessage
                        }).ToList());

                    throw new ValidationException(serviceResult);
                }
            }
            else
            {
                ServiceResult serviceResult = new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error()
                    {
                        Description = "Geçersiz parametre"
                    },
                    Validation = new Infrastructure.Communication.Moderator.Model.Validations.Validation()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItem>()
                    }
                };

                throw new ValidationException(serviceResult);
            }
        }
    }
}
