using System;

namespace MicroserviceProject.Presentation.UI.Infrastructure.Security.Model
{
    /// <summary>
    /// Oturumun token modeli
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Token anahtarı
        /// </summary>
        public string TokenKey { get; set; }

        /// <summary>
        /// Son geçerlilik tarihi
        /// </summary>
        public DateTime ValidTo { get; set; }
    }
}
