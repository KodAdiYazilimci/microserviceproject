using MicroserviceProject.Services.Transaction.Types;

namespace MicroserviceProject.Services.Transaction.Models
{
    /// <summary>
    /// Her bir öğe için geri alma modeli
    /// </summary>
    public class RollbackItemModel
    {
        /// <summary>
        /// Öğenin geri alma şekli
        /// </summary>
        public RollbackType RollbackType { get; set; }

        /// <summary>
        /// Öğenin bulunduğu veri seti
        /// </summary>
        public object DataSet { get; set; }

        /// <summary>
        /// İşlem öğesinin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Öğenin tanımlayıcısı
        /// </summary>
        public object Identity { get; set; }

        /// <summary>
        /// Öğenin önceki değeri
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Öğenin yeni değeri
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// İşlem öğesinin geri alınıp alınmadığı bilgisi
        /// </summary>
        public bool IsRolledback { get; set; }
    }
}
