
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;

using System;

namespace MicroserviceProject.Infrastructure.Communication.Moderator.Exceptions
{
    /// <summary>
    /// Doğrulama sorunu ortaya çıktığında fırlatılacak istisnai durum
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Doğrulama başarısız olması durumunda ortaya çıkacak servis sonucu
        /// </summary>
        private ServiceResult _serviceResult;

        /// <summary>
        /// Doğrulama başarısız olması durumunda ortaya çıkacak servis sonucu
        /// </summary>
        public ServiceResult ValidationResult
        {
            get
            {
                return _serviceResult;
            }
        }

        /// <summary>
        /// Doğrulama sorunu ortaya çıktığında fırlatılacak istisnai durum
        /// </summary>
        /// <param name="serviceResult">Doğrulama başarısız olması durumunda ortaya çıkacak servis sonucu</param>
        public ValidationException(ServiceResult serviceResult)
        {
            this._serviceResult = serviceResult;
        }
    }
}
