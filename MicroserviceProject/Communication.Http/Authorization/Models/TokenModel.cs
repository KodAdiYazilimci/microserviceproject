using System;

namespace Communication.Http.Authorization.Models
{
    /// <summary>
    /// Oturumun token modeli
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// Token anahtarı
        /// </summary>
        public string TokenKey { get; set; }

        /// <summary>
        /// Son geçerlilik tarihi
        /// </summary>
        public DateTime ValidTo { get; set; }
    }
}
