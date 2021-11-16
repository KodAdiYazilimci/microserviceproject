using Communication.Http.Department.Storage.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Storage.Configuration.Validation.Stock;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Storage.Util.Validation.Stock
{
    /// <summary>
    /// Stock/CreateStock Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateStockValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="stockModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(StockModel stockModel, CancellationTokenSource cancellationTokenSource)
        {
            CreateStockRule validationRules = new CreateStockRule();

            if (stockModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(stockModel, cancellationTokenSource.Token);

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
