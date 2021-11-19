using System;

namespace Infrastructure.Security.Model
{
    /// <summary>
    /// Oturumun token modeli
    /// </summary>
    public class AuthenticationToken
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
