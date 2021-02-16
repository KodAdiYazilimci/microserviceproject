using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Model.Validations;
using MicroserviceProject.Infrastructure.Security.Model;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Util.Validation.Auth.GetToken
{
    /// <summary>
    /// Auth/GetToken endpoint için validasyon
    /// </summary>
    public class GetTokenValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="credential">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResult> ValidateAsync(Credential credential, CancellationToken cancellationToken)
        {
            Configuration.Validation.Auth.GetToken.CredentialRule validationRules = new Configuration.Validation.Auth.GetToken.CredentialRule();

            if (credential != null)
            {
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
                        Validation = new  MicroserviceProject.Infrastructure.Communication.Model.Validations.Validation()
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
            else
            {
                ServiceResult serviceResult = new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error()
                    {
                        Description = "Geçersiz parametre"
                    },
                    Validation = new MicroserviceProject.Infrastructure.Communication.Model.Validations.Validation()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItem>()
                    }
                };

                return serviceResult;
            }
        }
    }
}
