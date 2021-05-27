
using Infrastructure.Validation.Models;

using System;

namespace Infrastructure.Validation.Exceptions
{
    /// <summary>
    /// Doğrulama sorunu ortaya çıktığında fırlatılacak istisnai durum
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Doğrulama başarısız olması durumunda ortaya çıkacak model sonucu
        /// </summary>
        private readonly ValidationModel _validationResult;

        /// <summary>
        /// Doğrulama başarısız olması durumunda ortaya çıkacak model sonucu
        /// </summary>
        public ValidationModel ValidationResult
        {
            get
            {
                return _validationResult;
            }
        }

        /// <summary>
        /// Doğrulama sorunu ortaya çıktığında fırlatılacak istisnai durum
        /// </summary>
        /// <param name="validation">Doğrulama başarısız olması durumunda ortaya çıkacak model sonucu</param>
        public ValidationException(ValidationModel validation)
        {
            this._validationResult = validation;
        }
    }
}
