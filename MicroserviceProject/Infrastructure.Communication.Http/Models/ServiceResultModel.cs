using Infrastructure.Validation.Models;

namespace Infrastructure.Communication.Http.Models
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
        public ValidationModel Validation { get; set; }

        /// <summary>
        /// Servis işlemleri boyunca geçerli olacak işlem kimliği
        /// </summary>
        public TransactionModel Transaction { get; set; }

        /// <summary>
        /// Yanıtın hangi serviste meydana geldiği
        /// </summary>
        public string SourceApiService { get; set; }
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
