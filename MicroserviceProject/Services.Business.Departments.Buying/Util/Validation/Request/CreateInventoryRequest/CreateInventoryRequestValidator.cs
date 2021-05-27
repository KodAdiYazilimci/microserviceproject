using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest;
using Services.Business.Departments.Buying.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest
{
    /// <summary>
    /// Request/CreateInventoryRequest Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateInventoryRequestValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="inventoryRequest">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(InventoryRequestModel inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            CreateInventoryRequestRule validationRules = new CreateInventoryRequestRule();

            if (inventoryRequest != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(inventoryRequest, cancellationTokenSource.Token);

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
