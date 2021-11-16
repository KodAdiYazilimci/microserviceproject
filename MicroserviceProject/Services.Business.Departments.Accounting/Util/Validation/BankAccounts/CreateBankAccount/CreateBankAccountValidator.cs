using Communication.Http.Department.Accounting.Models;

using FluentValidation.Results;

using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using Services.Business.Departments.Accounting.Configuration.Validation.BankAccounts.CreateBankAccount;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Accounting.Util.Validation.Department.CreateDepartment
{
    /// <summary>
    /// BankAccounts/CreateBankAccount Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateBankAccountValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="bankAccount">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(BankAccountModel bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            CreateBankAccountRule validationRules = new CreateBankAccountRule();

            if (bankAccount != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(bankAccount, cancellationTokenSource.Token);

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
