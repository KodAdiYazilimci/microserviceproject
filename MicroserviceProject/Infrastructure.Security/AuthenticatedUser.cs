using System.Collections.Generic;

namespace Infrastructure.Security.Model
{
    /// <summary>
    /// Oturumda bulunan kullanıcının modeli
    /// </summary>
    public class AuthenticatedUser
    {
        /// <summary>
        /// Kullanılacak bölge kodu
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Kullanıcının Id değeri
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Kullanıcının e-posta adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Kullanıcının parolası
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Kullanıcının oturum kimliği
        /// </summary>
        public int? SessionId { get; set; }

        /// <summary>
        /// Kullanıcının geçerli oturum anahtarı
        /// </summary>
        public AuthenticationToken Token { get; set; }

        /// <summary>
        /// Kullanıcı nitelikleri
        /// </summary>
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();

        /// <summary>
        /// Kullanıcı rolleri
        /// </summary>
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
