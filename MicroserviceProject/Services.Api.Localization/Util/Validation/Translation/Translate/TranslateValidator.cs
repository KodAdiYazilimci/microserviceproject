
using FluentValidation.Results;

using Infrastructure.Communication.Http.Models;
using Infrastructure.Validation.Models;

using Services.Api.Localization.Configuration.Validation.Translation.Translate;
using Services.Communication.Http.Broker.Localization.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Localization.Util.Validation.Translation.Translate
{
    /// <summary>
    /// Auth/GetToken endpoint için validasyon
    /// </summary>
    public class TranslateValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="translationModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResultModel> ValidateAsync(TranslationModel translationModel, CancellationTokenSource cancellationTokenSource)
        {
            TranslateRule validationRules = new TranslateRule();

            if (translationModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(translationModel, cancellationTokenSource.Token);

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
