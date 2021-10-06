using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Entities.EntityFramework
{
    public class ProductEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
