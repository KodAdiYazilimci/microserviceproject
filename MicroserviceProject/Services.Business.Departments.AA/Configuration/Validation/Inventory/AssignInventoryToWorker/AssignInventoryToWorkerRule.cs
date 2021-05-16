using FluentValidation;

using Services.Business.Departments.AA.Models;

using System.Linq;

namespace Services.Business.Departments.AA.Configuration.Validation.Inventory.AssignInventoryToWorker
{
    /// <summary>
    /// Inventory/AssignInventoryToWorker Http endpoint için validasyon kuralı
    /// </summary>
    public class AssignInventoryToWorkerRule : AbstractValidator<WorkerModel>
    {
        /// <summary>
        /// Inventory/AssignInventoryToWorker Http endpoint için validasyon kuralı
        /// </summary>
        public AssignInventoryToWorkerRule()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Çalışan Id si boş geçilemez");
            RuleFor(x => x.AAInventories).NotNull().WithMessage("Envanter bilgisi boş geçilemez");
            RuleFor(x => x.AAInventories.Any()).NotEqual(false).WithMessage("Envanter bilgisi boş geçilemez");
        }
    }
}
