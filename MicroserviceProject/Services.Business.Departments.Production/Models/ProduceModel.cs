using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Models
{
    public class ProduceModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
    }
}
