﻿using FluentValidation.Results;

using Infrastructure.Communication.Model.Department.AA;
using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Model;
using Services.Business.Departments.AA.Configuration.Validation.Inventory.CreateInventory;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.AA.Util.Validation.Inventory.CreateInventory
{
    /// <summary>
    /// Inventory/CreateInventory Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateInventoryValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="inventory">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(InventoryModel inventory, CancellationTokenSource cancellationTokenSource)
        {
            CreateInventoryRule validationRules = new CreateInventoryRule();

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