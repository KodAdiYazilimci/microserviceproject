namespace Services.Api.Infrastructure.Authorization.Constants
{
    /// <summary>
    /// Token talep tipleri
    /// </summary>
    public class GrantType
    {
        /// <summary>
        /// Parola
        /// </summary>
        public static string Password => "password";

        /// <summary>
        /// Yenileme tokenı
        /// </summary>
        public static string RefreshToken => "refresh_token";
    }
}
