using FluentValidation.Results;

using MicroserviceProject.Model.Communication.Basics;
using MicroserviceProject.Model.Communication.Errors;
using MicroserviceProject.Model.Communication.Validations;
using MicroserviceProject.Model.Security;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Util.Validation.Auth.GetToken
{
    public class GetTokenValidator
    {
        public static async Task<ServiceResult> ValidateAsync(Credential credential, CancellationToken cancellationToken)
        {
            Configuration.Validation.Auth.GetToken.CredentialRule validationRules = new Configuration.Validation.Auth.GetToken.CredentialRule();
            ValidationResult validationResult = await validationRules.ValidateAsync(credential, cancellationToken);

            if (!validationResult.IsValid)
            {
                ServiceResult serviceResult = new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error()
                    {
                        Description = "Geçersiz parametre"
                    },
                    Validation = new Model.Communication.Validations.Validation()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItem>()
                    }
                };
                serviceResult.Validation.ValidationItems.AddRange(
                    validationResult.Errors.Select(x => new ValidationItem()
                    {
                        Key = x.PropertyName,
                        Value = x.AttemptedValue,
                        Message = x.ErrorMessage
                    }).ToList());

                return serviceResult;
            }

            return new ServiceResult() { IsSuccess = true };
        }
    }
}
