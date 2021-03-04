using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Validation.Exceptions;
using MicroserviceProject.Infrastructure.Validation.Model;
using MicroserviceProject.Services.Business.Departments.Finance.Configuration.Validation.Cost.CreateCost;
using MicroserviceProject.Services.Model.Department.Finance;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Finance.Util.Validation.Cost.CreateCost
{
    /// <summary>
    /// Cpst/CreateCost Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateCostValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="costModel">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task ValidateAsync(DecidedCostModel costModel, CancellationToken cancellationToken)
        {
            CreateCostRule validationRules = new CreateCostRule();

            if (costModel != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(costModel, cancellationToken);

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
