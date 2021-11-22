namespace Services.Communication.Mq.Rabbit.Publisher.Department.Finance.Models
{
    /// <summary>
    /// Ürün üretim talebi modeli
    /// </summary>
    public class ProductionRequestModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
        public int ReferenceNumber { get; set; }
        public bool Approved { get; set; }
    }
}
