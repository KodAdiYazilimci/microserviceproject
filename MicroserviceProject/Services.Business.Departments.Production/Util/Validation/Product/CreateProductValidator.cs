﻿using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Production.Configuration.Validation.Selling;
using Services.Business.Departments.Production.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Util.Validation.Selling
{
    /// <summary>
    /// Product/CreateProduct Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateProductValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="productModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(ProductModel productModel, CancellationTokenSource cancellationTokenSource)
        {
            CreateProductRule validationRules = new CreateProductRule();

            if (productModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(productModel, cancellationTokenSource.Token);

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
