using Services.Api.Business.Departments.Production.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Api.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductionEntity : BaseEntity
    {
        public int DepartmentId { get; set; }
        public int ProductId { get; set; }
        public int RequestedAmount { get; set; }
        public int StatusId { get; set; }
        public int ReferenceNumber { get; set; }

        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<ProductionItemEntity> ProductionItems { get; set; }

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
