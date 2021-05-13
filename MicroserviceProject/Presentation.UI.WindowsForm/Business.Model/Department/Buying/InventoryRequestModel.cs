namespace Presentation.UI.WindowsForm.Business.Model.Department.Buying
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

        public override string ToString()
        {
            string revoke = Revoked ? "Onaylandı" : "Red";
            string close = Done ? "Kapandı" : "Açık";

            if (AAInventory != null)
                return $"{AAInventory.Name}{Amount}Adet (AA)-{revoke}&{close}";
            else if (ITInventory != null)
                return $"{ITInventory.Name}{Amount}Adet (AA)-{revoke}&{close}";
            else
                return "";
        }
    }
}
