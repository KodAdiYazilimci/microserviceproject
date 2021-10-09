using System.Collections.Generic;

namespace Services.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual ICollection<ProductDependencyEntity> ProductDependenciesForProduct { get; set; }
        public virtual ICollection<ProductDependencyEntity> ProductDependenciesForDependency { get; set; }
    }
}
