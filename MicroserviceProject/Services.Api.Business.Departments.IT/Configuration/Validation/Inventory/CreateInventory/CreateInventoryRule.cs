using FluentValidation;

using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.CreateInventory
{
    /// <summary>
    /// Inventory/CreateInventory Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateInventoryRule : AbstractValidator<ITInventoryModel>
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
