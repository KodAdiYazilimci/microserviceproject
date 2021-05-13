using FluentValidation;

using Services.Model.Department.Finance;

namespace Services.Business.Departments.Finance.Configuration.Validation.Cost.CreateCost
{
    /// <summary>
    /// Cost/CreateCost Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateCostRule : AbstractValidator<DecidedCostModel>
    {
        /// <summary>
        /// Cost/CreateCost Http endpoint için validasyon kuralı
        /// </summary>
        public CreateCostRule()
        {
            RuleFor(x => x.InventoryRequestId).NotEmpty().WithMessage("Talep Id geçersiz");
        }
    }
}
