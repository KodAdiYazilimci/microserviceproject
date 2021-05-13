using Infrastructure.Communication.Model.Department.Buying;

namespace Services.Model.Department.Finance
{
    public class DecidedCostModel
    {
        public int Id { get; set; }
        public int InventoryRequestId { get; set; }
        public bool Approved { get; set; }
        public bool Done { get; set; }

        public InventoryRequestModel InventoryRequest { get; set; }
    }
}
