using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Validation.Model;
using MicroserviceProject.Services.Logging.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging.Util.Validation.Logging.WriteRequestResponseLog
{
    /// <summary>
    /// Logging/WriteRequestResponseLog Http endpoint için validasyon
    /// </summary>
    public static class WriteRequestResponseLogValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="credential">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResultModel> ValidateAsync(RequestResponseLogModel credential, CancellationToken cancellationToken)
        {
            Configuration.Validation.Logging.WriteRequestResponseLog.RequestResponseLogModelRule validationRules =
                new Configuration.Validation.Logging.WriteRequestResponseLog.RequestResponseLogModelRule();

            if (credential != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(credential, cancellationToken);

                if (!validationResult.IsValid)
                {
                    ServiceResultModel serviceResult = new ServiceResultModel()
                    {
                        IsSuccess = false,
                        ErrorModel = new ErrorModel()
                        {
                            Description = "Geçersiz parametre"
                        },
                        Validation = new ValidationModel()
                        {
                            IsValid = false,
                            ValidationItems = new List<ValidationItemModel>()
                        }
                    };
                    serviceResult.Validation.ValidationItems.AddRange(
                        validationResult.Errors.Select(x => new ValidationItemModel()
                        {
                            Key = x.PropertyName,
                            Value = x.AttemptedValue,
                            Message = x.ErrorMessage
                        }).ToList());

                    return serviceResult;
                }

                return new ServiceResultModel() { IsSuccess = true };
            }
            else
            {
                ServiceResultModel serviceResult = new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel()
                    {
                        Description = "Geçersiz parametre"
                    },
                    Validation = new ValidationModel()
                    {
                        IsValid = false,
                        ValidationItems = new List<ValidationItemModel>()
                    }
                };

                return serviceResult;
            }
        }
    }
}
