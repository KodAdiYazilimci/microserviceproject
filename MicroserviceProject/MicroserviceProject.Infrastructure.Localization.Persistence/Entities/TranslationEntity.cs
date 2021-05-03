using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceProject.Infrastructure.Localization.Persistence.Entities
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
        public string Key { get; private set; }

        /// <summary>
        /// Çevirinin anahtar değerini tanımlar
        /// </summary>
        /// <param name="key">Çevirinin anahtar değeri</param>
        public void SetKey(string key)
        {
            SaveLog(
                new LogEntity(
                dbTable: nameof(TranslationEntity),
                columnName: nameof(Key),
                primaryKey: Id,
                oldValue: Key,
                newValue: key,
                logType: LogTypes.Update
                ));

            Key = key;
        }

        /// <summary>
        /// Çeviri metni
        /// </summary>
        [Column("TEXT", TypeName = "NVARCHAR(4000)")]
        [StringLength(maximumLength: 4000, ErrorMessage = "", MinimumLength = 2)]
        public string Text { get; private set; }

        /// <summary>
        /// Çevirinin metnini tanımlar
        /// </summary>
        /// <param name="text">Çevirinin metni</param>
        public void SetText(string text)
        {
            SaveLog(
                new LogEntity(
                dbTable: nameof(TranslationEntity),
                columnName: nameof(Text),
                primaryKey: Id,
                oldValue: Text,
                newValue: text,
                logType: LogTypes.Update
                ));

            Text = text;
        }

        /// <summary>
        /// Çevirinin dili
        /// </summary>
        [Column("LANGUAGECODE", TypeName = "NVARCHAR(50)")]
        [StringLength(maximumLength: 50, ErrorMessage = "", MinimumLength = 2)]
        public string LanguageCode { get; private set; }

        /// <summary>
        /// Çevirinin dil kodunu tanımlar
        /// </summary>
        /// <param name="languageCode">Çevirinin dil kodu</param>
        public void SetLanguageCode(string languageCode)
        {
            SaveLog(
                new LogEntity(
                dbTable: nameof(TranslationEntity),
                columnName: nameof(LanguageCode),
                primaryKey: Id,
                oldValue: LanguageCode,
                newValue: languageCode,
                logType: LogTypes.Update
                ));

            LanguageCode = languageCode;
        }

        /// <summary>
        /// Çeviriyi günceller
        /// </summary>
        /// <param name="entity">Güncel çeviri örneği</param>
        public override void Update(object entity)
        {
            TranslationEntity translation = entity as TranslationEntity;

            SetKey(translation.Key);
            SetLanguageCode(translation.LanguageCode);
            SetText(translation.Text);
        }
    }
}
