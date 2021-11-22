using System;

namespace Services.Api.Infrastructure.Authorization.Entities.EntityFramework
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

        public int? BeforeSessionId { get; set; }

        /// <summary>
        /// Oturum özel anahtarı
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Yenileme anahtarı
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Yenileme indeksi
        /// </summary>
        public int RefreshIndex { get; set; }

        public string GrantType { get; set; }
        public string Scope { get; set; }

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
