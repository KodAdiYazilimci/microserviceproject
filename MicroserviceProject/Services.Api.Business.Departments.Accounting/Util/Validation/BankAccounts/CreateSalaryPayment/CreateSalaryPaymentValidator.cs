using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Api.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount;
using Services.Communication.Http.Broker.Department.Accounting.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment
{
    /// <summary>
    /// BankAccounts/CreateSalaryPayment Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateSalaryPaymentValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="salaryPayment">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(AccountingSalaryPaymentModel salaryPayment, CancellationTokenSource cancellationTokenSource)
        {
            CreateSalaryPaymentRule validationRules = new CreateSalaryPaymentRule();

            if (salaryPayment != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(salaryPayment, cancellationTokenSource.Token);

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
