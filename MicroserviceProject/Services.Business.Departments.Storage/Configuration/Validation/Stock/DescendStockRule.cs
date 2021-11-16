using Communication.Http.Department.Storage.Models;

using FluentValidation;

namespace Services.Business.Departments.Storage.Configuration.Validation.Stock
{
    /// <summary>
    /// Stock/DescendProductStock Http endpoint için validasyon kuralı
    /// </summary>
    public class DescendStockRule : AbstractValidator<StockModel>
    {
        /// <summary>
        /// Stock/DescendProductStock Http endpoint için validasyon kuralı
        /// </summary>
        public DescendStockRule()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Ürün Id boş geçilemez");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Stok miktarı sıfırdan büyük olmalı");
        }
    }
}
