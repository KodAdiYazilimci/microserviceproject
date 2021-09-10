using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.CR.Configuration.Validation.Customer.CreateCustomer;
using Services.Business.Departments.CR.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.CR.Util.Validation.Customer.CreateCustomer
{
    /// <summary>
    /// Customer/CreateCustomer Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateCustomerValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="customer">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(CustomerModel customer, CancellationTokenSource cancellationTokenSource)
        {
            CreateCustomerRule validationRules = new CreateCustomerRule();

            if (customer != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(customer, cancellationTokenSource.Token);

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
