using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceProject.Services.Security.Authorization.Persistence.Exceptions
{
    /// <summary>
    /// Oturum bilgisi bulunamadı istisnası
    /// </summary>
    public class SessionNotFoundException : Exception
    {
        /// <summary>
        /// Exception mesajı
        /// </summary>
        private readonly string ErrorMessage = "Oturum bilgisi bulunamadı!";

        /// <summary>
        /// Oturum bilgisi bulunamadı istisnası
        /// </summary>
        public SessionNotFoundException()
        {

        }

        /// <summary>
        /// Oturum bilgisi bulunamadı istisnası
        /// </summary>
        /// <param name="errorMessage">Exception mesajı</param>
        public SessionNotFoundException(string errorMessage)
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
