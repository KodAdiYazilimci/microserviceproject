using Communication.Http.Department.Finance.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Finance.Configuration.Validation.Cost.DecideCost;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Cost.DecideCost
{
    /// <summary>
    /// Cpst/DecideCost Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class DecideCostValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="costModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(DecidedCostModel costModel, CancellationTokenSource cancellationTokenSource)
        {
            DecideCostRule validationRules = new DecideCostRule();

            if (costModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(costModel, cancellationTokenSource.Token);

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
