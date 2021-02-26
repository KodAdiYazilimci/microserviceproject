using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Validation.Exceptions;
using MicroserviceProject.Infrastructure.Validation.Model;
using MicroserviceProject.Services.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest;
using MicroserviceProject.Services.Model.Department.Buying;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Buying.Util.Validation.Request.CreateInventoryRequest
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
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(InventoryRequestModel inventoryRequest, CancellationToken cancellationToken)
        {
            CreateInventoryRequestRule validationRules = new CreateInventoryRequestRule();

            if (inventoryRequest != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(inventoryRequest, cancellationToken);

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
