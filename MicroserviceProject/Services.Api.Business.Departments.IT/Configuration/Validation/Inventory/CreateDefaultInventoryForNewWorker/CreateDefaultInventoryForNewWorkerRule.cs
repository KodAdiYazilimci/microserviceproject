using FluentValidation;

using Services.Communication.Http.Broker.Department.IT.Models;

namespace Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.CreateDefaultInventoryForNewWorker
{
    /// <summary>
    /// Inventory/CreateDefaultInventoryForNewWorker Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateDefaultInventoryForNewWorkerRule : AbstractValidator<ITInventoryModel>
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
