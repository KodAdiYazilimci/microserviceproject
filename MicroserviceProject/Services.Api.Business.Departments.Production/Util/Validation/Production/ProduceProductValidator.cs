using Services.Communication.Http.Broker.Department.Production.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Api.Business.Departments.Production.Configuration.Validation.Production;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Util.Validation.Production
{
    /// <summary>
    /// Production/ProduceProduct Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class ProduceProductValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="produceModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(ProduceModel produceModel, CancellationTokenSource cancellationTokenSource)
        {
            ProduceProductRule validationRules = new ProduceProductRule();

            if (produceModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(produceModel, cancellationTokenSource.Token);

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
