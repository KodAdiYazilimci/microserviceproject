using System;

namespace Services.Infrastructure.Authorization.Entities.EntityFramework
{
    /// <summary>
    /// Kullanıcı oturumları modeli
    /// </summary>
    public class Session : BaseEntity
    {
        /// <summary>
        /// Oturumun IP adresi
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Oturumun geçerlilik durumu
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Kullanıcının oturum bölgesi
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Oturum özel anahtarı
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Oturumun agent bilgisi
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Oturum sahibi kullanıcının Id değeri
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Oturumun son geçerlilik tarihi
        /// </summary>
        public DateTime ValidTo { get; set; }

        public virtual User User { get; set; }
    }
}
