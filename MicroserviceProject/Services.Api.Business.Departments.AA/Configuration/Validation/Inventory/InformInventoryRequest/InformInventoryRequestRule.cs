using Services.Communication.Http.Broker.Department.AA.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.AA.Configuration.Validation.Inventory.InformInventoryRequest
{
    /// <summary>
    /// Inventory/InformInventoryRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class InformInventoryRequestRule : AbstractValidator<AAInventoryRequestModel>
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
