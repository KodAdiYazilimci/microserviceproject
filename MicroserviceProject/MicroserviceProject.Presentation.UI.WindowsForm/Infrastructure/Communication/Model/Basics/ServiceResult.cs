using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Validations;
using MicroserviceProject.Presentation.UI.WindowsForm.Infrastructure.Communication.Model;

namespace MicroserviceProject.Presentation.UI.Infrastructure.Communication.Model.Basics
{
    /// <summary>
    /// Bir servisten dönen yanıt
    /// </summary>
    public class ServiceResultModel
    {
        /// <summary>
        /// Yanıtın başarılı olma durumu
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Yanıta konu olan hata
        /// </summary>
        public ErrorModel ErrorModel { get; set; }

        /// <summary>
        /// Yanıta konu olan doğrulama
        /// </summary>
        public Validation Validation { get; set; }

        /// <summary>
        /// Servis işlemleri boyunca geçerli olacak işlem kimliği
        /// </summary>
        public Transaction Transaction{ get; set; }
    }

    /// <summary>
    /// Bir servisten dönen yanıt
    /// </summary>
    /// <typeparam name="TResult">Servisten dönecek datanın tipi</typeparam>
    public class ServiceResultModel<TResult> : ServiceResultModel
    {
        /// <summary>
        /// Yanıtın içerisindeki data
        /// </summary>
        public TResult Data { get; set; }
    }
}
