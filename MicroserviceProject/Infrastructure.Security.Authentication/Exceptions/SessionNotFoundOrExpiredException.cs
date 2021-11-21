using System;

namespace Infrastructure.Security.Authentication.Exceptions
{
    /// <summary>
    /// Oturum bilgisi bulunamadı veya sona erdi istisnası
    /// </summary>
    public class SessionNotFoundOrExpiredException : Exception
    {
        /// <summary>
        /// Exception mesajı
        /// </summary>
        private readonly string ErrorMessage = "Oturum bilgisi bulunamadı veya sona erdi";

        /// <summary>
        /// Oturum bilgisi bulunamadı veya sona erdi istisnası
        /// </summary>
        public SessionNotFoundOrExpiredException()
        {

        }

        /// <summary>
        /// Oturum bilgisi bulunamadı istisnası
        /// </summary>
        /// <param name="errorMessage">Exception mesajı</param>
        public SessionNotFoundOrExpiredException(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage = errorMessage;
            }
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
