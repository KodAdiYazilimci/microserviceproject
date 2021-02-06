using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Model.Validations;
using MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment;
using MicroserviceProject.Services.Business.Model.Department.HR;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Department.CreateDepartment
{
    /// <summary>
    /// Department/CreateDepartment Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateDepartmentValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="department">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResult> ValidateAsync(DepartmentModel department, CancellationToken cancellationToken)
        {
            CreateDepartmentRule validationRules = new CreateDepartmentRule();

            if (department != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(department, cancellationToken);

                if (!validationResult.IsValid)
                {
                    ServiceResult serviceResult = new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = "Geçersiz parametre"
                        },
                        Validation = new Infrastructure.Communication.Model.Validations.Validation()
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

                    return serviceResult;
                }

                return new ServiceResult() { IsSuccess = true };
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
                    Validation = new Infrastructure.Communication.Model.Validations.Validation()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItem>()
                    }
                };

                return serviceResult;
            }
        }
    }
}
