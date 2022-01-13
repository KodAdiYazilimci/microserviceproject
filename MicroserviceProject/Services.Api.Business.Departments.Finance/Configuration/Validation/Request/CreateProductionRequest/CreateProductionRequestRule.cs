using FluentValidation;

using Services.Communication.Http.Broker.Department.Finance.Models;

namespace Services.Business.Departments.Finance.Configuration.Validation.Request.CreateProductionRequest
{
    /// <summary>
    /// Request/CreateProductionRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateProductionRequestRule : AbstractValidator<ProductionRequestModel>
    {
        /// <summary>
        /// Request/CreateProductionRequest Http endpoint için validasyon kuralı
        /// </summary>
        public CreateProductionRequestRule()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Talep edilen miktar geçersiz");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Talebi ileten departman geçersiz");
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Üretilmesi istenilen ürün geçersiz");
            RuleFor(x => x.ReferenceNumber).GreaterThan(0).WithMessage("Referans numarası geçersiz");
        }
    }
}
