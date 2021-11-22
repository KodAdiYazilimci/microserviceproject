using Services.Communication.Http.Broker.Department.Storage.Models;

using FluentValidation;

namespace Services.Business.Departments.Storage.Configuration.Validation.Stock
{
    /// <summary>
    /// Stock/CreateStock Http endpoint için validasyon kuralı
    /// </summary>
    public class CreateStockRule : AbstractValidator<StockModel>
    {
        /// <summary>
        /// Stock/CreateStock Http endpoint için validasyon kuralı
        /// </summary>
        public CreateStockRule()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Stok miktarı sıfırdan büyük olmalı");
        }
    }
}
