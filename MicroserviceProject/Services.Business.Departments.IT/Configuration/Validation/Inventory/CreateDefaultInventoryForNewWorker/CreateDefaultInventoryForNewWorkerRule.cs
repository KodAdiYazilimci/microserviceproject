using FluentValidation;

using Infrastructure.Communication.Model.Department.IT;

namespace Services.Business.Departments.IT.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker
{
    /// <summary>
    /// Inventory/CreateDefaultInventoryForNewWorker Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateDefaultInventoryForNewWorkerRule : AbstractValidator<InventoryModel>
    {
        /// <summary>
        /// Inventory/CreateInventory Http endpoint için validasyon kuralı
        /// </summary>
        public CreateDefaultInventoryForNewWorkerRule()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Envanter Id boş geçilemez");
        }
    }
}
