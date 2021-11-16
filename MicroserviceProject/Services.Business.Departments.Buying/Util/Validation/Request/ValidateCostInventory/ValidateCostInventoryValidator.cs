using Communication.Http.Department.Buying.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Buying.Configuration.Validation.Request.ValidateCostInventory;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Util.Validation.Request.ValidateCostInventory
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
