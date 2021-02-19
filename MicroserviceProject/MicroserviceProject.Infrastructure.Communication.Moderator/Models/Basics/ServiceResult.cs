using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Validations;

namespace MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics
{
    /// <summary>
    /// Bir servisten dönen yanıt
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Yanıtın başarılı olma durumu
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Yanıta konu olan hata
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// Yanıta konu olan doğrulama
        /// </summary>
        public Validation Validation { get; set; }
    }

    /// <summary>
    /// Bir servisten dönen yanıt
    /// </summary>
    /// <typeparam name="TResult">Servisten dönecek datanın tipi</typeparam>
    public class ServiceResult<TResult> : ServiceResult
    {
        /// <summary>
        /// Yanıtın içerisindeki data
        /// </summary>
        public TResult Data { get; set; }
    }
}
