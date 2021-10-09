using Services.Business.Departments.Production.Constants;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductionEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public int ReferenceNumber { get; set; }
        public int DepartmentId { get; set; }
        public int StatusId { get; set; }

        public virtual ProductEntity Product { get; set; }

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
