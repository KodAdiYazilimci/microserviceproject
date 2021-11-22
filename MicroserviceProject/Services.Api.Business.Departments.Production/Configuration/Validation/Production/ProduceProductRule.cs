using Services.Communication.Http.Broker.Department.Production.Models;

using FluentValidation;

namespace Services.Api.Business.Departments.Production.Configuration.Validation.Production
{
    /// <summary>
    /// Production/ProduceProduct Http endpoint için validasyon kuralı
    /// </summary>
    public class ProduceProductRule : AbstractValidator<ProduceModel>
    {
        /// <summary>
        /// Production/ProduceProduct Http endpoint için validasyon kuralı
        /// </summary>
        public ProduceProductRule()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Departman Id boş geçilemez");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalıdır");
            RuleFor(x => x.ReferenceNumber).GreaterThan(0).WithMessage("Referans numarası sıfırdan büyük olmalıdır");
        }
    }
}
