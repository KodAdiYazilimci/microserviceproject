using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Validation.Exceptions;
using MicroserviceProject.Infrastructure.Validation.Model;
using MicroserviceProject.Services.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest;
using MicroserviceProject.Services.Business.Departments.Buying.Configuration.Validation.Request.ValidateCostInventory;
using MicroserviceProject.Services.Model.Department.Finance;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory
{
    /// <summary>
    /// Request/ValidateCostInventory Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class ValidateCostInventoryValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="decidedCost">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(DecidedCostModel decidedCost, CancellationTokenSource cancellationTokenSource)
        {
            ValidateCostInventoryRule validationRules = new ValidateCostInventoryRule();

            if (decidedCost != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(decidedCost, cancellationTokenSource.Token);

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
