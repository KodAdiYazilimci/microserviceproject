using FluentValidation;

using Services.Business.Departments.Finance.Models;

namespace Services.Business.Departments.Finance.Configuration.Validation.Request.DecideProductionRequest
{
    /// <summary>
    /// Request/CreateProductionRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class DecideProductionRequestRule : AbstractValidator<ProductionRequestModel>
    {
        /// <summary>
        /// Request/CreateProductionRequest Http endpoint için validasyon kuralı
        /// </summary>
        public DecideProductionRequestRule()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Talep Id geçersiz");
        }
    }
}
