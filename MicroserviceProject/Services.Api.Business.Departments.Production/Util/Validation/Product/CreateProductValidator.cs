using Infrastructure.Validation;

using Services.Api.Business.Departments.Production.Configuration.Validation.Product;
using Services.Communication.Http.Broker.Department.Production.Models;

using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Util.Validation.Product
{
    /// <summary>
    /// Product/CreateProduct Http endpoint için validasyon kuralını doğrulayan sınıf
    /// </summary>
    public class CreateProductValidator : BaseValidator<ProductModel, CreateProductRule>
    {
        public CreateProductValidator(CreateProductRule validationRule) : base(validationRule)
        {
        }

        public override async Task ValidateAsync(ProductModel entity, CancellationTokenSource cancellationTokenSource)
        {
            if (entity == null)
            {
                ThrowDefaultValidationException();
            }

            await base.ValidateAsync(entity, cancellationTokenSource);
        }
    }
}
