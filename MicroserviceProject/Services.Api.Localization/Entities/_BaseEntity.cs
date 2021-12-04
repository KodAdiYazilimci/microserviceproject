using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Api.Localization.Entities
{
    /// <summary>
    /// Tüm ortak entityler için base sınıf
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Id değeri
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "INT")]
        public int Id { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>        
        [Column("CREATEDATE", TypeName = "DATETIME")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Güncelleme tarihi
        /// </summary>        
        [Column("UPDATEDATE", TypeName = "DATETIME")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Silinme tarihi
        /// </summary>        
        [Column("DELETEDATE", TypeName = "DATETIME")]
        public DateTime? DeleteDate { get; set; }
    }
}
