﻿using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Departments.HR.Entities.Sql
{
    /// <summary>
    /// Ünvanlar tablosu entity sınıfı
    /// </summary>
    public class TitleEntity : BaseEntity
    {
        /// <summary>
        /// Ünvanın adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ünvana ait çalışan kayıtları
        /// </summary>
        public virtual ICollection<WorkerEntity> Workers { get; set; }
    }
}
