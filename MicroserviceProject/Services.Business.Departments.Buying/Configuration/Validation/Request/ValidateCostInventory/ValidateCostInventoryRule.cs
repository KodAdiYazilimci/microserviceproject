using Services.Communication.Http.Broker.Department.Buying.Models;

using FluentValidation;

namespace Services.Business.Departments.Buying.Configuration.Validation.Request.ValidateCostInventory
{
    /// <summary>
    /// Request/ValidateCostInventory Http endpoint için validasyon kuralı
    /// </summary>
    public class ValidateCostInventoryRule : AbstractValidator<DecidedCostModel>
    {
        /// <summary>
        /// Inventory/ValidateCostInventory Http endpoint için validasyon kuralı
        /// </summary>
        public ValidateCostInventoryRule()
        {
            RuleFor(x => x.InventoryRequestId).NotEmpty().WithMessage("Envanter talep Id si boş geçilemez");
        }
    }
}
