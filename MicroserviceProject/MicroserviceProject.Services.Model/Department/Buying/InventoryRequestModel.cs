namespace MicroserviceProject.Services.Model.Department.Buying
{
    public class InventoryRequestModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int DepartmentId { get; set; }
        public int Amount { get; set; }
        public bool Revoked { get; set; }
        public bool Done { get; set; }

        public AA.InventoryModel AAInventory { get; set; }
        public IT.InventoryModel ITInventory { get; set; }
    }
}
