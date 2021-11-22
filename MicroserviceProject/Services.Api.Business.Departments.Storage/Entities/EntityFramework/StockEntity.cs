using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Entities.EntityFramework
{
    public class StockEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
