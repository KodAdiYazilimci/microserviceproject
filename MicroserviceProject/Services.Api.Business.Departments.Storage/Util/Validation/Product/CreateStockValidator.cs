using Infrastructure.Validation;

using Services.Api.Business.Departments.Storage.Configuration.Validation.Stock;
using Services.Communication.Http.Broker.Department.Storage.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Util.Validation.Stock
{
    /// <summary>
    /// Stock/CreateStock Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateStockValidator : BaseValidator<StockModel, CreateStockRule>
    {
        public CreateStockValidator(CreateStockRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(StockModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
