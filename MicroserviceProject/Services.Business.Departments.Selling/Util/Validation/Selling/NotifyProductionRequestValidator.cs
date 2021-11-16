using Communication.Http.Department.Selling.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Selling.Configuration.Validation.Selling;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Selling.Util.Validation.Selling
{
    /// <summary>
    /// Selling/NotifyProductionRequestValidator Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class NotifyProductionRequestValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="productionRequestModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(ProductionRequestModel productionRequestModel, CancellationTokenSource cancellationTokenSource)
        {
            NotifyProductionRequestRule validationRules = new NotifyProductionRequestRule();

            if (productionRequestModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(productionRequestModel, cancellationTokenSource.Token);

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
