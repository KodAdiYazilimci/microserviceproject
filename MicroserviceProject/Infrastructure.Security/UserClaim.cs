namespace Infrastructure.Security.Model
{
    /// <summary>
    /// Kullanıcı niteliği sınıfı
    /// </summary>
    public class UserClaim
    {
        /// <summary>
        /// Niteliğin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Niteliğin değeri
        /// </summary>
        public string Value { get; set; }
    }
}
