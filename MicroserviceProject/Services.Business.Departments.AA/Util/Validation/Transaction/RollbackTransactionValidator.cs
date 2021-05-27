using FluentValidation.Results;

using Infrastructure.Transaction.Recovery;
using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.AA.Configuration.Validation.Transaction;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.AA.Util.Validation.Transaction
{
    /// <summary>
    /// Transaction/RollbackTransaction Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class RollbackTransactionValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="rollbackModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(RollbackModel rollbackModel, CancellationTokenSource cancellationTokenSource)
        {
            RollbackTransactionRule validationRules = new RollbackTransactionRule();

            if (rollbackModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(rollbackModel, cancellationTokenSource.Token);

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
