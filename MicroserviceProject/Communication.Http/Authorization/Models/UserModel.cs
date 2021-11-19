using System.Collections.Generic;

namespace Communication.Http.Authorization.Models
{
    /// <summary>
    /// Kullanıcının modeli
    /// </summary>
    public class UserModel
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
        public TokenModel Token { get; set; }

        /// <summary>
        /// Kullanıcı nitelikleri
        /// </summary>
        public List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();

        /// <summary>
        /// Kullanıcı rolleri
        /// </summary>
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}
