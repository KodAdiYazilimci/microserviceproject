namespace Services.Communication.Mq.Rabbit.Department.Models.Selling
{
    /// <summary>
    /// Ürün üretim talebi modeli
    /// </summary>
    public class ProductionRequestQueueModel : BaseQueueModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int DepartmentId { get; set; }
        public int ReferenceNumber { get; set; }
        public bool Approved { get; set; }
    }
}
