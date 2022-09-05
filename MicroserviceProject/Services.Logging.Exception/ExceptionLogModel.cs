using Infrastructure.Logging.Model;

namespace Services.Logging.Exception.Configuration
{
    /// <summary>
    /// Exception log modeli
    /// </summary>
    public class ExceptionLogModel : BaseLogModel
    {
        /// <summary>
        /// Exception mesajı
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// İç exception mesajı
        /// </summary>
        public string InnerExceptionMessage { get; set; }
    }
}
