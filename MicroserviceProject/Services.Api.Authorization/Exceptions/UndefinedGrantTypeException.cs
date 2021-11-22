using System;

namespace Services.Api.Infrastructure.Authorization.Persistence.Sql.Exceptions
{
    /// <summary>
    /// Belirsiz token talep tipi istisnası
    /// </summary>
    public class UndefinedGrantTypeException : Exception
    {
        /// <summary>
        /// Exception mesajı
        /// </summary>
        private readonly string ErrorMessage = "Tanımsız token talep tipi";

        /// <summary>
        /// Belirsiz token talep tipi istisnası
        /// </summary>
        public UndefinedGrantTypeException()
        {

        }

        /// <summary>
        /// Belirsiz token talep tipi istisnası
        /// </summary>
        /// <param name="errorMessage">Exception mesajı</param>
        public UndefinedGrantTypeException(string errorMessage)
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
