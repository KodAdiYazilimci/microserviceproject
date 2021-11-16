using Services.Business.Departments.Selling.Constants;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Business.Departments.Selling.Entities.EntityFramework
{
    public class SellEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public int SellStatusId { get; set; }
        public int ReferenceNumber { get; set; }

        [NotMapped]
        public SellStatus SellStatus
        {
            get
            {
                return (SellStatus)Enum.ToObject(typeof(SellStatus), (byte)SellStatusId);
            }
        }
    }
}
