using Services.Business.Departments.Production.Constants;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductionItemEntity : BaseEntity
    {
        public int ProductionId { get; set; }
        public int DependedProductId { get; set; }
        public int RequiredAmount { get; set; }
        public int StatusId { get; set; }

        public virtual ProductEntity DependedProduct { get; set; }
        public virtual ProductionEntity Production { get; set; }

        [NotMapped]
        public ProductionStatus ProductionStatus
        {
            get
            {
                return (ProductionStatus)Enum.ToObject(typeof(ProductionStatus), StatusId);
            }
        }
    }
}
