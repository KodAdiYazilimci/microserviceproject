using Services.Communication.Http.Broker.Department.Production.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.Production.Configuration.Validation.Product
{
    /// <summary>
    /// Product/CreateProduct Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateProductRule : AbstractValidator<ProductModel>
    {
        /// <summary>
        /// Product/CreateProduct Http endpoint için validasyon kuralı
        /// </summary>
        public CreateProductRule()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adı boş geçilemez");
        }
    }
}
