using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Validations;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreatePerson;
using MicroserviceProject.Services.Model.Department.HR;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroserviceProject.Infrastructure.Communication.Moderator.Exceptions;

namespace MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreatePerson
{
    /// <summary>
    /// Person/CreatePerson Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreatePersonValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="person">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(PersonModel person, CancellationToken cancellationToken)
        {
            CreatePersonRule validationRules = new CreatePersonRule();

            if (person != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(person, cancellationToken);

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
