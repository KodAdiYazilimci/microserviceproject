using Services.Communication.Http.Broker.Department.Selling.Models;

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
    /// Selling/CreateSelling Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateSellingValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="sellModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(SellModel sellModel, CancellationTokenSource cancellationTokenSource)
        {
            CreateSellingRule validationRules = new CreateSellingRule();

            if (sellModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(sellModel, cancellationTokenSource.Token);

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
