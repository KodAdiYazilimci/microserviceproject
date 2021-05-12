using System;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Exceptions
{
    /// <summary>
    /// Kullanıcı bulunamadı istisnası
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Exception mesajı
        /// </summary>
        private readonly string ErrorMessage = "Kullanıcı bulunamadı!";

        /// <summary>
        /// Kullanıcı bulunamadı istisnası
        /// </summary>
        public UserNotFoundException()
        {

        }

        /// <summary>
        /// Kullanıcı bulunamadı istisnası
        /// </summary>
        /// <param name="errorMessage">Exception mesajı</param>
        public UserNotFoundException(string errorMessage)
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
