using FluentValidation.Results;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Model.Validations;
using MicroserviceProject.Services.Business.Departments.HR.Configuration.Validation.Person.CreateWorker;
using MicroserviceProject.Services.Model.Department.HR;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Util.Validation.Person.CreateWorker
{
    /// <summary>
    /// Person/CreateWorker Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateWorkerValidator
    {
        /// <summary>
        /// Request body doğrular
        /// </summary>
        /// <param name="worker">Doğrulanacak nesne</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public static async Task<ServiceResult> ValidateAsync(WorkerModel worker, CancellationToken cancellationToken)
        {
            CreateWorkerRule validationRules = new CreateWorkerRule();

            if (worker != null)
            {
                ValidationResult validationResult = await validationRules.ValidateAsync(worker, cancellationToken);

                if (!validationResult.IsValid)
                {
                    ServiceResult serviceResult = new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = "Geçersiz parametre"
                        },
                        Validation = new Infrastructure.Communication.Model.Validations.Validation()
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
                    Validation = new Infrastructure.Communication.Model.Validations.Validation()
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
