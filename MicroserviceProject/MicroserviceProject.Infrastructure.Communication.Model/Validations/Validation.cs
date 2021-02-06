﻿using System.Collections.Generic;

namespace MicroserviceProject.Infrastructure.Communication.Model.Validations
{
    /// <summary>
    /// Servisten dönen doğrulama
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// Doğrulamanın geçerli olup olmadığı
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Doğrulamaya ait detaylar
        /// </summary>
        public List<ValidationItem> ValidationItems { get; set; }
    }
}