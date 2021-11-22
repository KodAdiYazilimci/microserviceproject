using Services.Communication.Http.Broker.Department.IT.Models;

using FluentValidation;

namespace Services.Business.Departments.IT.Configuration.Validation.Inventory.InformInventoryRequest
{
    /// <summary>
    /// Inventory/InformInventoryRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class InformInventoryRequestRule : AbstractValidator<InventoryRequestModel>
    {
        /// <summary>
        /// Inventory/InformInventoryRequest Http endpoint için validasyon kuralı
        /// </summary>
        public InformInventoryRequestRule()
        {
            RuleFor(x => x.InventoryId).NotEmpty().WithMessage("Envanter Id boş geçilemez");
        }
    }
}
