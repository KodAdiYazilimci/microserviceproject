using FluentValidation.Results;

using Infrastructure.Communication.Model.Department.HR;
using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Model;
using Services.Business.Departments.HR.Configuration.Validation.Department.CreateDepartment;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.HR.Util.Validation.Department.CreateDepartment
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
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(DepartmentModel department, CancellationTokenSource cancellationTokenSource)
        {
            CreatePersonRule validationRules = new CreatePersonRule();

            if (department != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(department, cancellationTokenSource.Token);

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
