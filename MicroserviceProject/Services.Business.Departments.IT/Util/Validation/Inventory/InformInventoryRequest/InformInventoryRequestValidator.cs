﻿using FluentValidation.Results;

using Infrastructure.Communication.Model.Department.Buying;
using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Model;
using Services.Business.Departments.IT.Configuration.Validation.Inventory.InformInventoryRequest;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.IT.Util.Validation.Inventory.InformInventoryRequest
{
    /// <summary>
    /// Inventory/InformInventoryRequestValidator Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class InformInventoryRequestValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="inventory">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(InventoryRequestModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            InformInventoryRequestRule validationRules = new InformInventoryRequestRule();

            if (inventory != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(inventory, cancellationTokenSource.Token);

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