
using FluentValidation;

using Services.Communication.Http.Broker.Department.IT.Models;

using System.Linq;

namespace Services.Api.Business.Departments.IT.Configuration.Validation.Inventory.AssignInventoryToWorker
{
    /// <summary>
    /// Inventory/AssignInventoryToWorker Http endpoint için validasyon kuralı
    /// </summary>
    public class AssignInventoryToWorkerRule : AbstractValidator<ITAssignInventoryToWorkerModel>
    {
        /// <summary>
        /// Inventory/AssignInventoryToWorker Http endpoint için validasyon kuralı
        /// </summary>
        public AssignInventoryToWorkerRule()
        {
            RuleFor(x => x.InventoryId).NotNull().WithMessage("Envanter bilgisi boş geçilemez");
            RuleFor(x => x.WorkerId).NotEmpty().WithMessage("Çalışan Id si boş geçilemez");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Miktar bilgisi boş geçilemez");
        }
    }
}
