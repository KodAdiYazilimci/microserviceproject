using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Api.ServiceDiscovery.Configuration.Validation.Registry.Register;
using Services.Api.ServiceDiscovery.Dto;

namespace Services.Api.ServiceDiscovery.Util.Validation.Registry.Register
{
    public class RegisterValidator
    {
        public static async Task ValidateAsync(ServiceDto serviceDto, CancellationTokenSource cancellationTokenSource)
        {
            RegisterRule validationRules = new RegisterRule();

            if (serviceDto != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(serviceDto, cancellationTokenSource.Token);

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
