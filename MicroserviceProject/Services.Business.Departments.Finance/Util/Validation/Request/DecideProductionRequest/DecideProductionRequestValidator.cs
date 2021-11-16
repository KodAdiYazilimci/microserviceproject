using Communication.Http.Department.Finance.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Finance.Configuration.Validation.Request.DecideProductionRequest;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Util.Validation.Request.CreateProductionRequest
{
    /// <summary>
    /// Request/DecideProductionRequest Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class DecideProductionRequestValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="productionRequest">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(ProductionRequestModel productionRequest, CancellationTokenSource cancellationTokenSource)
        {
            DecideProductionRequestRule validationRules = new DecideProductionRequestRule();

            if (productionRequest != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(productionRequest, cancellationTokenSource.Token);

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
