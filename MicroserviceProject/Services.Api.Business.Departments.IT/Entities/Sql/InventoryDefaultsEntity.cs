namespace Services.Api.Business.Departments.IT.Entities.Sql
{
    /// <summary>
    /// AA ye ait envanterlerin varsayılanları entity sınıfı
    /// </summary>
    public class InventoryDefaultsEntity : BaseEntity
    {
        /// <summary>
        /// Varsayılanın ait olduğu envanterin Id değeri
        /// </summary>
        public int InventoryId { get; set; }

        /// <summary>
        /// Varsayılan bilgisinin yeni çalışan için olup olmadığı
        /// </summary>
        public bool ForNewWorker { get; set; }

        /// <summary>
        /// Varsayılanın ait olduğu envanter
        /// </summary>
        public virtual InventoryEntity Inventory { get; set; }
    }
}
