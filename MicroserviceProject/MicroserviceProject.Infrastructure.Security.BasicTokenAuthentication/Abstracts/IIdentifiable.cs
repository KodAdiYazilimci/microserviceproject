using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceProject.Infrastructure.Security.BasicTokenAuthentication.Abstracts
{
    /// <summary>
    /// Kimliği tanımlayacak arayüz
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Kimlik tanımlayıcısı
        /// </summary>
        Guid Identifier { get; }
    }
}
