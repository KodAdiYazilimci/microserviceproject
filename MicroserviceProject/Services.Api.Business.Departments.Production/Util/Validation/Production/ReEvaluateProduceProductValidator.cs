
using Infrastructure.Validation.Exceptions;
using Infrastructure.Validation.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Util.Validation.Production
{
    /// <summary>
    /// Production/ReEvaluateProduceProductValidator Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class ReEvaluateProduceProductValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="produceModel">Doğrulanacak nesne</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public static Task ValidateAsync(int referenceNumber, CancellationTokenSource cancellationTokenSource)
        {
            if (referenceNumber <= 0)
            {
                ValidationModel validation = new ValidationModel()
                {
                    IsValid = false,
                    ValidationItems = new List<ValidationItemModel>()
                };

                validation.ValidationItems.Add(
                    new ValidationItemModel()
                    {
                        Key = nameof(referenceNumber),
                        Message = "Geçersiz referans numarası",
                        Value = referenceNumber
                    });

                throw new ValidationException(validation);
            }

            return Task.CompletedTask;
        }
    }
}
