namespace Communication.Mq.Rabbit.Publisher.Department.Buying.Models
{
    public class ProductRequestModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int ReferenceNumber { get; set; }
    }
}
