namespace Services.Communication.Http.Broker.Department.IT.Models
{
    public class ITAssignInventoryToWorkerModel
    {
        public int InventoryId { get; set; }
        public int WorkerId { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
