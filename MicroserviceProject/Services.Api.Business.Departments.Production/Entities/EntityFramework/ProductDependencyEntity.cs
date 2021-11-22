namespace Services.Api.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductDependencyEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public int DependedProductId { get; set; }
        public int Amount { get; set; }

        public virtual ProductEntity Product { get; set; }
        public virtual ProductEntity DependedProduct { get; set; }
    }
}
