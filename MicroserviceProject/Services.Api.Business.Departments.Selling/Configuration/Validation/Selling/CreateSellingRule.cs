using Services.Communication.Http.Broker.Department.Selling.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.Selling.Configuration.Validation.Selling
{
    /// <summary>
    /// Selling/CreateSelling Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateSellingRule : AbstractValidator<SellModel>
    {
        /// <summary>
        /// Selling/CreateSelling Http endpoint için validasyon kuralı
        /// </summary>
        public CreateSellingRule()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Müşteri Id boş geçilemez");
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı");
        }
    }
}
