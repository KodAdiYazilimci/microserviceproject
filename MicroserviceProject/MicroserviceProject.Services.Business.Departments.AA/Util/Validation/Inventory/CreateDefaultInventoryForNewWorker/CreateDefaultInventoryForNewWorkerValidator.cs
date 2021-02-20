using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Validations;
using MicroserviceProject.Services.Business.Departments.AA.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker;
using MicroserviceProject.Services.Model.Department.AA;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Util.Validation.Inventory.CreateDefaultInventoryForNewWorker
{
    /// <summary>
    /// Inventory/CreateDefaultInventoryForNewWorker Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateDefaultInventoryForNewWorkerValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="inventory">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResult> ValidateAsync(InventoryModel inventory, CancellationToken cancellationToken)
        {
            CreateDefaultInventoryForNewWorkerRule validationRules = new CreateDefaultInventoryForNewWorkerRule();

            if (inventory != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(inventory, cancellationToken);

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
                    Validation = new Infrastructure.Communication.Moderator.Model.Validations.Validation()
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
