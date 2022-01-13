using FluentValidation;

using Services.Communication.Http.Broker.Department.Buying.Models;

namespace Services.Api.Business.Departments.Buying.Configuration.Validation.Request.CreateInventoryRequest
{
    /// <summary>
    /// Request/CreateInventoryRequest Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateInventoryRequestRule : AbstractValidator<InventoryRequestModel>
    {
        /// <summary>
        /// Inventory/AssignInventoryToWorker Http endpoint için validasyon kuralı
        /// </summary>
        public CreateInventoryRequestRule()
        {
            RuleFor(x => x.InventoryId).NotEmpty().WithMessage("Envanter Id si boş geçilemez");
            RuleFor(x => x.DepartmentId).NotNull().WithMessage("Departman Id si boş geçilemez");
            RuleFor(x => x.Amount).NotEqual(0).WithMessage("Miktar bilgisi boş geçilemez");
        }
    }
}
