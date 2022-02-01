using Infrastructure.Logging.Model;

namespace Services.Logging.Aspect
{
    /// <summary>
    /// Çalışma zamanı log modeli
    /// </summary>
    public class RuntimeLogModel : BaseLogModel
    {
        /// <summary>
        /// Çalışılacak methodun adı
        /// </summary>
        public string MethodName { get; set; } = string.Empty;

        /// <summary>
        /// Methoda verilen parametrelerin Json formatlı değeri
        /// </summary>
        public string ParametersAsJson { get; set; } = string.Empty;

        /// <summary>
        /// Methodun geri dönüş değerinin Json formatlı değeri
        /// </summary>
        public string ResultAsJson { get; set; } = string.Empty;
    }
}
