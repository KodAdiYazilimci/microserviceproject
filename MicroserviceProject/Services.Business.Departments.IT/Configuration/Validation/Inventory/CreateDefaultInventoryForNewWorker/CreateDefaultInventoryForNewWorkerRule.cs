using Communication.Http.Department.IT.Models;

using FluentValidation;

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
