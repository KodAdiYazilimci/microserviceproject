using System.Collections.Generic;

namespace MicroserviceProject.Infrastructure.Validation.Model
{
    /// <summary>
    /// Servisten dönen doğrulama
    /// </summary>
    public class ValidationModel
    {
        /// <summary>
        /// Doğrulamanın geçerli olup olmadığı
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Doğrulamaya ait detaylar
        /// </summary>
        public List<ValidationItemModel> ValidationItems { get; set; }
    }
}
