using FluentValidation;

using Services.Business.Departments.AA.Models;

namespace Services.Business.Departments.AA.Configuration.Validation.Inventory.CreateInventory
{
    /// <summary>
    /// Inventory/CreateInventory Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateInventoryRule : AbstractValidator<InventoryModel>
    {
        /// <summary>
        /// Inventory/CreateInventory Http endpoint için validasyon kuralı
        /// </summary>
        public CreateInventoryRule()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Envanter adı boş geçilemez");
        }
    }
}
