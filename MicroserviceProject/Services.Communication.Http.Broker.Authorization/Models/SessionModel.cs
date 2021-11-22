using System;

namespace Services.Communication.Http.Broker.Authorization.Models
{
    /// <summary>
    /// Kullanıcı oturumları modeli
    /// </summary>
    public class SessionModel
    {
        /// <summary>
        /// Oturumun Id değeri
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Oturumun IP adresi
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Oturumun geçerlilik durumu
        /// </summary>
        public bool IsValid { get; set; }

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
    }
}
