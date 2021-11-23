using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Localization.Translation.Persistence.EntityFramework.Entities
{
    /// <summary>
    /// Dil çevirileri entity sınıfı
    /// </summary>
    [Table("TRANSLATIONS")]
    public class TranslationEntity : BaseEntity
    {
        /// <summary>
        /// Çevirinin anahtar değeri
        /// </summary>
        [Column("KEY", TypeName = "NVARCHAR(250)")]
        [StringLength(maximumLength: 250, ErrorMessage = "", MinimumLength = 2)]
        public string Key { get; set; }

        /// <summary>
        /// Çeviri metni
        /// </summary>
        [Column("TEXT", TypeName = "NVARCHAR(4000)")]
        [StringLength(maximumLength: 4000, ErrorMessage = "", MinimumLength = 2)]
        public string Text { get; set; }

        /// <summary>
        /// Çevirinin dili
        /// </summary>
        [Column("LANGUAGECODE", TypeName = "NVARCHAR(50)")]
        [StringLength(maximumLength: 50, ErrorMessage = "", MinimumLength = 2)]
        public string LanguageCode { get; set; }
    }
}
