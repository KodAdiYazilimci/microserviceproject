namespace Infrastructure.Security.Model
{
    /// <summary>
    /// Kimlik doğrulama modeli
    /// </summary>
    public class AuthenticationCredential
    {
        /// <summary>
        /// Kullanıcının e-posta adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Kullanıcının parolası
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Http user agent değeri
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Kullanıcının IP adresi
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Kullanıcının bölge seçimi
        /// </summary>
        public string Region { get; set; }
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }
}
