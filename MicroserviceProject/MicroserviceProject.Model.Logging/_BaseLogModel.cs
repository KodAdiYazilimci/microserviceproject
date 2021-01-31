using System;

namespace MicroserviceProject.Model.Logging
{
    /// <summary>
    /// Yazılacak logların ortak modeli
    /// </summary>
    public abstract class BaseLogModel
    {
        /// <summary>
        /// Logun oluştuğu makinenin adı
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Logu oluşturan uygulamanın adı
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Logun içerik metni
        /// </summary>
        public string LogText { get; set; }

        /// <summary>
        /// Logun oluştuğu tarih
        /// </summary>
        public DateTime Date { get; set; }
    }
}
