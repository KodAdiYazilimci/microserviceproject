namespace MicroserviceProject.Infrastructure.Security.Model
{
    /// <summary>
    /// Kullanıcının modeli
    /// </summary>
    public class User
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
        /// Kullanıcının adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kullanıcının oturum kimliği
        /// </summary>
        public int? SessionId { get; set; }

        /// <summary>
        /// Kullanıcının geçerli oturum anahtarı
        /// </summary>
        public Token Token { get; set; }

        /// <summary>
        /// Kullanıcının e-posta adresi
        /// </summary>
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public string Password { get; set; }
    }
}
