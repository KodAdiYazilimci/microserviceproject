using Communication.Http.Department.Selling.Models;

using FluentValidation;

namespace Services.Business.Departments.Selling.Configuration.Validation.Selling
{
    /// <summary>
    /// Selling/NotifyProductionRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class NotifyProductionRequestRule : AbstractValidator<ProductionRequestModel>
    {
        /// <summary>
        /// Selling/CreateSelling Http endpoint için validasyon kuralı
        /// </summary>
        public NotifyProductionRequestRule()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.ReferenceNumber).GreaterThan(0).WithMessage("Geçersiz referans numarası");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Geçersiz miktar");
        }
    }
}
