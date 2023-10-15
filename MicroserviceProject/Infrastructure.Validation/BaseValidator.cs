using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Validation
{
    public class BaseValidator<TEntity, TValidationRule> where TValidationRule : FluentValidation.AbstractValidator<TEntity>
    {
        private readonly TValidationRule _validationRule;

        public BaseValidator(TValidationRule validationRule)
        {
            _validationRule = validationRule;
        }

        public virtual async Task ValidateAsync(TEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            ValidationResult validationResult = await _validationRule.ValidateAsync(entity, cancellationTokenSource.Token);

            if (!validationResult.IsValid)
            {
                ValidationModel validationModel = new ValidationModel()
                {
                    IsValid = false,
                    ValidationItems = new List<ValidationItemModel>()
                };

                validationModel.ValidationItems.AddRange(
                    validationResult.Errors.Select(x => new ValidationItemModel()
                    {
                        Key = x.PropertyName,
                        Value = x.AttemptedValue,
                        Message = x.ErrorMessage
                    }).ToList());

                throw new ValidationException(validationModel);
            }
        }

        public void ThrowDefaultValidationException()
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
